using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ScheduleApp {
    public class Class : DatabaseRecord{

        #region Constructors
        public Class() { }

        public Class(SqlDataReader dr) {
            Fill(dr);
        }
        #endregion

        #region DB strings
        internal const string db_ID = "ClassID";
        internal const string db_Name = "Name";
        internal const string db_RoomNum = "RoomNum";
        internal const string db_BuildingNum = "BuildingNum";
        internal const string db_StartTime = "StartTime";
        internal const string db_EndTime = "EndTime";
        internal const string db_ProfName = "ProfName";
        #endregion

        #region Private variables
        private string _Name;
        private int _RoomNum;
        private int _BuildingNum;
        private DateTime _StartTime;
        private DateTime _EndTime;
        private string _ProfName;
        #endregion

        #region public Properties
        ///<summary>
        ///Gets or Sets Name
        /// </summary>
        public string Name {
            get {
                return Name;
            }
            set {
                _Name = value.Trim();
            }
        }

        ///<summary>
        ///Gets or Sets Room number
        /// </summary>
        public int RoomNum {
            get {
                return RoomNum;
            }
            set {
                _RoomNum = value;
            }
        }

        ///<summary>
        ///Get or Set Building Number
        /// </summary>
        public int BuildingNum {
            get {
                return BuildingNum;
            }
            set {
                _BuildingNum = value;
            }
        }

        ///<summary>
        ///Get or Set Start Time
        /// </summary>
        [Display(Name = "Time Started")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddThh:mm}", ApplyFormatInEditMode = true)]
        [JsonPropertyName("StartTime")]
        public DateTime StartTime {
            get {
                return StartTime;
            }
            set {
                _StartTime = value;
            }
        }

        ///<summary>
        ///Get or Set End Time
        /// </summary>
        public DateTime EndTime {
            get {
                return EndTime;
            }
            set {
                _EndTime = value;
            }
        }

        ///<summary>
        ///Get or Set Professor Name
        /// </summary>
        public string ProfName {
            get {
                return ProfName;
            }
            set {
                _ProfName = value.Trim();
            }
        }
        #endregion

        #region public functions
        public override int dbSave() {
            if (_ID < 0) {
                return dbAdd();
            } else {
                return dbUpdate();
            }
        }
        protected override int dbAdd() {
            _ID = DAL.AddClass(this);
            return _ID;
        }
        protected override int dbUpdate() {
            return DAL.UpdateClass(this);
        }
        #endregion

        public override void Fill(SqlDataReader dr) {
            _ID = (int)dr[db_ID];
            _Name = (string)dr[db_Name];
            _RoomNum = (int)dr[db_RoomNum];
            _StartTime = (DateTime)dr[db_StartTime];
            _EndTime = (DateTime)dr[db_EndTime];
            _ProfName = (string)dr[db_ProfName];
        }

        public override string ToString() {
            return String.Format("{0}", this.ID);
        }
    }
}
