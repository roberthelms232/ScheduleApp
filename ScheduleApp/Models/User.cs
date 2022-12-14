using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;

namespace ScheduleApp {
    public class User : DatabaseRecord, IComparable<User>, IComparable {

        #region Constructors
        public User() {
            }
        public User(SqlDataReader dr) {
            Fill(dr);
        }
        #endregion

        #region DB strings
        internal const string db_ID = "UserID";
        internal const string db_RoleID = "RoleID";
        internal const string db_FirstName = "FirstName";
        internal const string db_LastName = "LastName";
        internal const string db_Email = "Email";
        internal const string db_Password = "Password";
        #endregion

        #region Private Variables
        //These will be used to set the data 
        private int _RoleID;
        private Role _Role;
        private string _FirstName;
        private string _LastName;
        private string _Email;
        private string _Password;
        private string _ConfrimPassword;
        //private string _Salt; -- to be implemented if I have time to salt the passwords
        #endregion

        #region Public Properties
        ///<summary>
        ///Gets or Sets the Role associated with the User
        /// </summary>
        public Role Role {
            get {
                if (_Role == null && _RoleID > 0) {
                    //To be implemened after DAL is ready
                    //_Role = DAL.GetRole(_RoleID);
                }
                return Role;
            }
            set {
                _Role = value;
                if (value == null) {
                    _RoleID = -1;
                } else {
                    //Will be set up to pull from databaserecord
                    //_RoleID = value.ID;
                }
             }
         }

        ///<summary>
        ///Gets or Sets RoleID from database
        /// </summary>
        [Display(Name = "Role")]
        [Required(ErrorMessage = "Please select a role")]
        public int RoleID {
            get {
                return RoleID;
            }
            set {
                _RoleID = value;
            }
        }

        ///<summary>
        ///Gets or Sets FirstName
        /// </summary>
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please fill in your a first name!")]
        public string FirstName {
            get {
                return FirstName;
            }
            set {
                _FirstName = value.Trim();
            }
        }

        ///<summary>
        ///Gets or Sets LastName
        /// </summary>
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please fill in your last name!")]
        public string LastName {
            get {
                return LastName;
            }
            set {
                _LastName = value.Trim();
            }
        }

        ///<summary>
        ///Gets or Sets Email
        /// </summary>
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please fill in your email")]
        [DataType(DataType.EmailAddress)]
        public string Email {
            get {
                return Email;
            }
            set {
                _Email = value.Trim();
            }
        }

        ///<summary>
        ///Gets or Sets FullName
        /// </summary>
        public string FullName {
            get {
                return FirstName + " " + LastName;
            }
        }
        ///<summary>
        ///Gets or Sets Password
        /// </summary>
        [DataType(DataType.Password)]
        public string Password {
            get {
                return Password;
            }
            set {
                if(value != null) {
                    _Password = value.Trim();
                } else {
                    _Password = "";
                }
            }
        }
        ///<summary>
        ///Checks to make sure Passwords match
        /// </summary>
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword {
            get {
                return ConfirmPassword;
            }
            set {
                _ConfrimPassword = value;
            }
        }

        #endregion

        #region Public Functions
        //Need DAL Implementation to work
        public override int dbSave() {
            if (_ID < 0) {
                return dbAdd();
            } else {
                return dbUpdate();
            }
        }
        protected override int dbAdd() {
            _ID = DAL.AddUser(this);
            return _ID;
        }
        protected override int dbUpdate() {
            return DAL.UpdateUser(this);
        }
        #endregion

        public override void Fill(SqlDataReader dr) {
            _ID = (int)dr[db_ID];
            _RoleID = (int)dr[db_RoleID];
            _FirstName = (string)dr[db_FirstName];
            _LastName = (string)dr[db_LastName];
            _Email = (string)dr[db_Email];
            _Password = (string)dr[db_Password];

        }
        public override string ToString() {
            return String.Format("{0}", this.ID);
        }

        //Used for possible sorting in the future.
        public int CompareTo(User other) {
            return this.FirstName.CompareTo(other.FirstName);
        }

        public int CompareTo(object obj) {
            return this.FirstName.CompareTo(((User)obj).FirstName);
        }
    }
}
