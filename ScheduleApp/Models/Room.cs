using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;

namespace ScheduleApp {
    public class Room : DatabaseRecord {

        #region Constructors
        public Room() { }

        public Room(SqlDataReader dr) {
            Fill(dr);
        }
        #endregion

        #region DB Strings
        internal const string db_ID = "RoomNum";
        internal const string db_BuildingNum = "BuildingNum";
        #endregion

        #region Private Strings
        private int _RoomNum;
        private int _BuildingNum;
        #endregion

        #region Public Properties
        public int RoomNum {
            get {
                return RoomNum;
            }
            set {
                _RoomNum = value;
            }
        }

        public int BuildingNum {
            get {
                return BuildingNum;
            }
            set {
                _BuildingNum = value;
            }
        }
        #endregion

        #region Public Functions
        public override int dbSave() {
            if (_ID < 0) {
                return dbAdd();
            } else {
                return dbUpdate();
            }
        }
        protected override int dbAdd() {
            _ID = DAL.AddRoom(this);
            return _ID;
        }
        protected override int dbUpdate() {
            return DAL.UpdateRoom(this);
        }
        #endregion

        public override void Fill(SqlDataReader dr) {
            _ID = (int)dr[db_ID];
            _BuildingNum = (int)dr[db_BuildingNum];
        }

        public override string ToString() {
            return String.Format("{0}", this.ID);
        }
    }
}
