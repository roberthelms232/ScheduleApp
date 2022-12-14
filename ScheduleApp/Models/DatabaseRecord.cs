using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleApp {
    //Abstract class made for ease of access to DB
    public abstract class DatabaseRecord {
        protected int _ID = -1;

        [JsonIgnore]
        [Display(Name = "ID")]
        [Key]
        public int ID {
            get { return _ID; }
            set { _ID = value; }
        }

        public abstract int dbSave();

        protected abstract int dbAdd();

        protected abstract int dbUpdate();
        public abstract void Fill(Microsoft.Data.SqlClient.SqlDataReader dr);
        public abstract override string ToString();

        //Not Currently Functioning

        public abstract class DatabaseRecordNamed : DatabaseRecord {
            protected string _Name;
            /// <summary>
            /// The User given Name for the Object.
            /// </summary>
            //[Display(Name = "Name")]
            [DataType(DataType.Text)]
            //[Required]
            [JsonPropertyName("Name")]
            [Display(Name = "Name")]
            public String Name {
                get { return _Name; }
                set { _Name = value; }
            }
        }

    }
}
