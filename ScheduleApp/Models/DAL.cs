using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;

namespace ScheduleApp {
    public static class DAL {
        //Defines strings for purpose of getting or edditing data in database
        private static string _ReadOnlyConnectionString;
        private static string _EditOnlyConnectionString;
        private static int? dbcalls = null;

        internal enum dbAction {
            Read,
            Edit
        }

        #region Database Connections
        //This is to keep track of DB calls, QoL feature for later testing and cleaning up
        internal static void StartCounting() {
            dbcalls = 0;
        }
        //Returns the DB call count
        public static int GetCount() {
            int numb = 0;
            if (dbcalls != null) numb = (int)dbcalls;
            dbcalls = null;
            return numb;
        }
        //Creates a connection string to either get or edit data in the database
        internal static string ConnectionString(dbAction action = dbAction.Read) {
            string retString = "";
            if (action == dbAction.Read) retString = _ReadOnlyConnectionString;
            else retString = _EditOnlyConnectionString;
            return retString;
        }
        //Used to form a connection to the database
        internal static void ConnectToDatabase(SqlCommand comm, dbAction action = dbAction.Read) {
            try {
                comm.Connection = new SqlConnection(ConnectionString(action));

                comm.CommandType = System.Data.CommandType.StoredProcedure;
            }catch(Exception ex) {
                ShowErrorMessage(ex);
            }
        }
        //Used to get data from the database
        public static SqlDataReader GetDataReader(SqlCommand comm) {
            try {
                ConnectToDatabase(comm);
                comm.Connection.Open();
                if (dbcalls != null) dbcalls++;
                System.Diagnostics.Debug.WriteLine("DB Called (" + dbcalls + "): " + comm.CommandText);
                return comm.ExecuteReader();
            }catch(Exception ex) {
                ShowErrorMessage(ex);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return null;
            }
        }

        internal static int AddObject(SqlCommand comm, string parameterName) {

            int retInt = 0;

            try {
                ConnectToDatabase(comm, dbAction.Edit);
                comm.Connection.Open();
                SqlParameter retParameter;
                retParameter = comm.Parameters.Add(parameterName, System.Data.SqlDbType.Int);
                retParameter.Direction = System.Data.ParameterDirection.Output;
                comm.ExecuteNonQuery();
                retInt = (int)retParameter.Value;
                comm.Connection.Close();
            }catch(Exception ex) {
                ShowErrorMessage(ex);
                if (comm.Connection != null)
                    comm.Connection.Close();

                retInt = 0;
                ShowErrorMessage(ex);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return retInt;
        }

        internal static int UpdateObject(SqlCommand comm) {
            int retInt = 0;
            try {
                ConnectToDatabase(comm, dbAction.Edit);
                comm.Connection.Open();
                retInt = (int)comm.ExecuteNonQuery();
                comm.Connection.Close();
            }catch(Exception ex) {
                ShowErrorMessage(ex);
                if (comm.Connection != null)
                    comm.Connection.Close();

                retInt = 0;
                ShowErrorMessage(ex);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return retInt;
        }
        #endregion

        #region Dynamic Error Messaging 

        //Used to create error messages while getting or editing informtion on the database
        private static void ShowErrorMessage(Exception ex) {
            MethodBase site = ex.TargetSite;
            StackTrace trace = new StackTrace(false);
            string messageStart = "==DAL Error==";
            if(site != null) {
                messageStart = site.DeclaringType.ToString() + "." + site.Name;
            }
            Debug.WriteLine(messageStart + ": " + ex.Message);
        }
        #endregion

        #region User
        ///<summary>
        ///Gets all the Users in the database
        /// </summary>
        /// <returns>A list of users</returns>
        internal static List<User> GetUsers() {
            List<User> retList = new List<User>();
            SqlCommand comm = new SqlCommand("sprocUsersGetAll");
            try {
                SqlDataReader dr = GetDataReader(comm);
                while (dr.Read()) {
                    retList.Add(new User(dr));
                }
            }catch(Exception ex) {
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.Message);
            } finally {
                if (comm.Connection != null) comm.Connection.Close();
            }
            return retList;
        }

        ///<summary>
        ///Gets a User by ID from the database
        /// </summary>
        /// <returns>A single user</returns>
        internal static User GetUser(int id) {
            User retObj = null;
            SqlCommand comm = new SqlCommand("sprocUserGet");
            try {
                comm.Parameters.AddWithValue("@UserID", id);
                SqlDataReader dr = GetDataReader(comm);
                while (dr.Read()) {
                    retObj = new User(dr);
                }
            }catch(Exception ex) {
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.Message);
            } finally {
                if(comm.Connection != null) comm.Connection.Close();
            }
            return retObj;
        }
        //Gets a specific user by the UserName
        internal static User GetUserByUserName(string uName) {
            SqlCommand comm = new SqlCommand("sprocUserGetByEmail");
            User retObj = null;
            try {
                comm.Parameters.AddWithValue("@" + User.db_Email, uName);
                SqlDataReader dr = GetDataReader(comm);
                while (dr.Read()) {
                    retObj = new User(dr);
                }
            } catch (Exception ex) {
                ShowErrorMessage(ex);
            } finally {
                if (comm.Connection != null)
                    comm.Connection.Close();
            }
            return retObj;
        }
        //Used for the purpose of a Login, retrieves a user by the Username and checks if the password provided matches one in the DB
        internal static User GetUser(string uName, string pWord) {
            User usr = GetUserByUserName(uName);
            if(usr != null && usr.Password != null) {
                if(usr.Password == pWord) {
                    //Password is good
                } else {
                    //Password doesn't match
                    usr = null;
                }
            } else {
                usr = null;
            }
            return usr;
        }
        //Creates a Cookie for a user when they Login
        public static string GetCookie(User usr) {
            return "!!!" + usr.ID + "!$!";
        }
        //Adds a User to the DB
        internal static int AddUser(User obj) {
            int retAnswer = -1;
            SqlCommand comm = new SqlCommand("sproc_UserAdd");
            try {
                comm.Parameters.AddWithValue("@" + User.db_RoleID, obj.RoleID);
                comm.Parameters.AddWithValue("@" + User.db_FirstName, obj.FirstName);
                comm.Parameters.AddWithValue("@" + User.db_LastName, obj.LastName);
                comm.Parameters.AddWithValue("@" + User.db_Email, obj.Email);
                comm.Parameters.AddWithValue("@" + User.db_Password, obj.Password);

                retAnswer = AddObject(comm, "@" + User.db_ID);
            }catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.Message);
            } finally {
                if (comm.Connection != null) comm.Connection.Close();
            }
            return retAnswer;
        }
        //Updates an existing user
        internal static int UpdateUser(User obj) {
            int retAnswer = -1;
            SqlCommand comm = new SqlCommand("sproc_UserUpdate");
            try {
                comm.Parameters.AddWithValue("@" + User.db_ID, obj.ID);
                comm.Parameters.AddWithValue("@" + User.db_RoleID, obj.RoleID);
                comm.Parameters.AddWithValue("@" + User.db_FirstName, obj.FirstName);
                comm.Parameters.AddWithValue("@" + User.db_LastName, obj.LastName);
                comm.Parameters.AddWithValue("@" + User.db_Email, obj.Email);
                comm.Parameters.AddWithValue("@" + User.db_Password, obj.Password);

                retAnswer = UpdateObject(comm);
            }catch(Exception ex) {
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.Message);
            } finally {
                if (comm.Connection != null) comm.Connection.Close();
            }
            return retAnswer;
        }

        #endregion

        #region Role
        //Gets all roles from the DB
        internal static List<Role> GetRoles() {
            List<Role> retList = new List<Role>();
            SqlCommand comm = new SqlCommand("SprocRolesGetAll");

            try {
                SqlDataReader dr = GetDataReader(comm);
                while (dr.Read()) {
                    retList.Add(new Role(dr));
                }
            }catch(Exception ex) {
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.Message);
            } finally {
                if (comm.Connection != null) comm.Connection.Close();
            }
            return retList;
        }
        //Gets a specific Role 
        internal static Role GetRole(int id) {
            Role retObj = null;
            SqlCommand comm = new SqlCommand("sprocRoleGet");
            try {
                comm.Parameters.AddWithValue("@RoleID", id);
                SqlDataReader dr = GetDataReader(comm);
                while (dr.Read()) {
                    retObj = new Role(dr);
                }
            }catch(Exception ex) {
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.Message);
            } finally {
                if (comm.Connection != null) comm.Connection.Close();
            }
            return retObj;
        }
        //Adds a role to the DB
        internal static int AddRole(Role obj) {
            int retAnswer = -1;
            SqlCommand comm = new SqlCommand("sproc_RoleAdd");
            try {
                comm.Parameters.AddWithValue("@" + Role.db_Name, obj.Name);
                comm.Parameters.AddWithValue("@" + Role.db_IsAdmin, obj.IsAdmin);
                comm.Parameters.AddWithValue("@" + Role.db_CanEditProfile, obj.CanEditProfile);
                comm.Parameters.AddWithValue("@" + Role.db_Users, obj.UsersPermissions.AsByte);
                comm.Parameters.AddWithValue("@" + Role.db_Roles, obj.Roles.AsByte);
                comm.Parameters.AddWithValue("@" + Role.db_Rooms, obj.Rooms.AsByte);
                comm.Parameters.AddWithValue("@" + Role.db_Classes, obj.Classes.AsByte);

                retAnswer = AddObject(comm, "@" + Role.db_ID);
            }catch(Exception ex) {
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.Message);
            } finally {
                if (comm.Connection != null) comm.Connection.Close();
            }
            return retAnswer;
        }
        //Updates an existing role
        internal static int UpdateRole(Role obj) {
            int retAnswer = -1;
            SqlCommand comm = new SqlCommand("Sproc_RoleUpdate");
            try {
                comm.Parameters.AddWithValue("@" + Role.db_Name, obj.Name);
                comm.Parameters.AddWithValue("@" + Role.db_IsAdmin, obj.IsAdmin);
                comm.Parameters.AddWithValue("@" + Role.db_CanEditProfile, obj.CanEditProfile);
                comm.Parameters.AddWithValue("@" + Role.db_Users, obj.UsersPermissions.AsByte);
                comm.Parameters.AddWithValue("@" + Role.db_Roles, obj.Roles.AsByte);
                comm.Parameters.AddWithValue("@" + Role.db_Rooms, obj.Rooms.AsByte);
                comm.Parameters.AddWithValue("@" + Role.db_Classes, obj.Classes.AsByte);

                retAnswer = UpdateObject(comm);
            }catch(Exception ex) {
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.Message);
            } finally {
                if (comm.Connection != null) comm.Connection.Close();
            }
            return retAnswer;
        }
        #endregion

        #region Room
        //Gets all rooms in the DB
        internal static List<Room> GetRooms() {
            List<Room> retList = new List<Room>();
            SqlCommand comm = new SqlCommand("SprocRoomsGetAll");

            try {
                SqlDataReader dr = GetDataReader(comm);
                while (dr.Read()) {
                    retList.Add(new Room(dr));
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.Message);
            } finally {
                if (comm.Connection != null) comm.Connection.Close();
            }
            return retList;
        }
        //Gets a specific room from the DB
        internal static Room GetRoom(int id) {
            Room retObj = null;
            SqlCommand comm = new SqlCommand("sprocRoomGet");
            try {
                comm.Parameters.AddWithValue("@RoomID", id);
                SqlDataReader dr = GetDataReader(comm);
                while (dr.Read()) {
                    retObj = new Room(dr);
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.Message);
            } finally {
                if (comm.Connection != null) comm.Connection.Close();
            }
            return retObj;
        }

        internal static int AddRoom(Room obj) {
            int retAnswer = -1;
            SqlCommand comm = new SqlCommand("sproc_RoomAdd");
            try {
                comm.Parameters.AddWithValue("@" + Room.db_BuildingNum, obj.BuildingNum);

                retAnswer = AddObject(comm, "@" + Room.db_ID);
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.Message);
            } finally {
                if (comm.Connection != null) comm.Connection.Close();
            }
            return retAnswer;
        }

        internal static int UpdateRoom(Room obj) {
            int retAnswer = -1;
            SqlCommand comm = new SqlCommand("Sproc_RoomUpdate");
            try {
                comm.Parameters.AddWithValue("@" + Room.db_BuildingNum, obj.BuildingNum);
                retAnswer = UpdateObject(comm);
            }catch(Exception ex) {
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.Message);
            } finally {
                if (comm.Connection != null) comm.Connection.Close();
            }
            return retAnswer;
        }
        #endregion

        #region Class
        internal static List<Class> GetClasses() {
            List<Class> retList = new List<Class>();
            SqlCommand comm = new SqlCommand("SprocClassesGetAll");

            try {
                SqlDataReader dr = GetDataReader(comm);
                while (dr.Read()) {
                    retList.Add(new Class(dr));
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.Message);
            } finally {
                if (comm.Connection != null) comm.Connection.Close();
            }
            return retList;
        }

        internal static Class GetClass(int id) {
            Class retObj = null;
            SqlCommand comm = new SqlCommand("sprocClassGet");
            try {
                comm.Parameters.AddWithValue("@ClassID", id);
                SqlDataReader dr = GetDataReader(comm);
                while (dr.Read()) {
                    retObj = new Class(dr);
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.Message);
            } finally {
                if (comm.Connection != null) comm.Connection.Close();
            }
            return retObj;
        }

        internal static int AddClass(Class obj) {
            int retAnswer = -1;
            SqlCommand comm = new SqlCommand("sproc_ClassAdd");
            try {
                comm.Parameters.AddWithValue("@" + Class.db_Name, obj.Name);
                comm.Parameters.AddWithValue("@" + Class.db_RoomNum, obj.RoomNum);
                comm.Parameters.AddWithValue("@" + Class.db_BuildingNum, obj.BuildingNum);
                comm.Parameters.AddWithValue("@" + Class.db_StartTime, obj.StartTime);
                comm.Parameters.AddWithValue("@" + Class.db_EndTime, obj.EndTime);
                comm.Parameters.AddWithValue("@" + Class.db_ProfName, obj.ProfName);

                retAnswer = AddObject(comm, "@" + Class.db_ID);
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.Message);
            } finally {
                if (comm.Connection != null) comm.Connection.Close();
            }
            return retAnswer;
        }

        internal static int UpdateClass(Class obj) {
            int retAnswer = -1;
            SqlCommand comm = new SqlCommand("Sproc_ClassUpdate");
            try {
                comm.Parameters.AddWithValue("@" + Class.db_Name, obj.Name);
                comm.Parameters.AddWithValue("@" + Class.db_RoomNum, obj.RoomNum);
                comm.Parameters.AddWithValue("@" + Class.db_BuildingNum, obj.BuildingNum);
                comm.Parameters.AddWithValue("@" + Class.db_StartTime, obj.StartTime);
                comm.Parameters.AddWithValue("@" + Class.db_EndTime, obj.EndTime);
                comm.Parameters.AddWithValue("@" + Class.db_ProfName, obj.ProfName);

                retAnswer = UpdateObject(comm);
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("ERROR: " + ex.Message);
            } finally {
                if (comm.Connection != null) comm.Connection.Close();
            }
            return retAnswer;
        }
        #endregion

        #region Set Settings
        //Sets the connection strings
        public static void Set(string readConnectionString, string editConnectionString) {
            _ReadOnlyConnectionString = readConnectionString;
            _EditOnlyConnectionString = editConnectionString;
        }
        #endregion

    }
}
