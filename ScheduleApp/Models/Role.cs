using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;

namespace ScheduleApp {
    public class Role : DatabaseRecord, IComparable, IComparable<Role> {

        #region Constructors
        public Role() {

        }
        public Role(SqlDataReader dr) {
            Fill(dr);
        }
        #endregion

        #region DB Strings
        internal const string db_ID = "RoleID";
        internal const string db_Name = "Name";
        internal const string db_IsAdmin = "IsAdmin";
        internal const string db_CanEditProfile = "CanEditProfile";
        internal const string db_Roles = "Roles";
        internal const string db_Users = "Users";
        internal const string db_Rooms = "Rooms";
        internal const string db_Classes = "Classes";
        #endregion

        #region Private Variables
        //Default role has no permissions
        private string _Name;
        private bool _IsAdmin = false;
        private bool _CanEditProfile = false;

        private DAVE _Roles = new DAVE(0);
        private DAVE _Rooms = new DAVE(0);
        private DAVE _Classes = new DAVE(0);
        private DAVE _UsersPermissions = new DAVE(0);
        #endregion

        #region Enums
        public enum Actions {
            Delete = 8,
            Add = 4,
            View = 2,
            Edit = 1
        }
        #endregion

        #region public Properties
        /// <summary>
        /// This is used to determine if user is an Admin
        /// </summary>
        [UIHint("YesNo")]
        public bool IsAdmin {
            get {
                return _IsAdmin;
            }
            set {
                _IsAdmin = value;
            }
        }
        [UIHint("YesNo")]
        public bool CanEditProfile {
            get {
                return _IsAdmin || _CanEditProfile;
            }
            set {
                _CanEditProfile = value;
            }
        }

        public string Name {
            get { return _Name; }
            set { _Name = value; }
        }

        public DAVE Roles {
            get {
                if(IsAdmin && _Roles.AsByte != 15) {
                    _Roles = new DAVE(15);
                }
                return _Roles;
            }
            set {
                _Roles = value;
            }
        }
        public DAVE Rooms {
            get {
                if (IsAdmin && _Rooms.AsByte != 15) {
                    _Rooms = new DAVE(15);
                }
                return _Rooms;
            }
            set {
                _Rooms = value;
            }
        }
        public DAVE Classes {
            get {
                if (IsAdmin && _Classes.AsByte != 15) {
                    _Classes = new DAVE(15);
                }
                return _Classes;
            }
            set {
                _Classes = value;
            }
        }

        public DAVE UsersPermissions {
            get {
                if(IsAdmin && _UsersPermissions.AsByte != 15) {
                    _UsersPermissions = new DAVE(15);
                }
                return _UsersPermissions;
            }
            set {
                _UsersPermissions = value;
            }
        }
        #endregion

        #region Public Functions
        public override int dbSave() {
            if(_ID < 0) {
                return dbAdd();
            } else {
                return dbUpdate();
            }
        }
        protected override int dbAdd() {
            _ID = DAL.AddRole(this);
            return _ID;
        }
        protected override int dbUpdate() {
            return DAL.UpdateRole(this);
        }
        #endregion

        public override void Fill(SqlDataReader dr) {
            _ID = (int)dr[db_ID];
            _Name = (string)dr[db_Name];
            _IsAdmin = (bool)dr[db_IsAdmin];
            _CanEditProfile = (bool)dr[db_CanEditProfile];
            _Roles = new DAVE((byte)dr[db_Roles]);
            _Rooms = new DAVE((byte)dr[db_Roles]);
            _Classes = new DAVE((byte)dr[db_Classes]);
        }
        public override string ToString() {
            return String.Format("{0}", this.ID);
        }
        //Used for Possible sorting in future iterations
        public int CompareTo(Role other) {
            return this.Name.CompareTo(other.Name);
        }

        public int CompareTo(object obj) {
            return this.Name.CompareTo(((Role)obj).Name);
        }
    }
}
