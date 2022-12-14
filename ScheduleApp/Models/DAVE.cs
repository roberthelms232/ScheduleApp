using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

//Code taken from previous project, writen with help from Jon Holmes

namespace ScheduleApp {
    // Possible DAVE Values
    // 0001 = 1  -                          Edit
    // 0010 = 2  -                  View
    // 0011 = 3  -                  View    Edit
    // 0100 = 4  -          Add
    // 0101 = 5  -          Add	            Edit
    // 0110 = 6  -          Add	    View
    // 0111 = 7  -          Add	    View	Edit
    // 1000 = 8  - Delete
    // 1001 = 9  - Delete	                Edit
    // 1010 = 10 - Delete	        View
    // 1011 = 11 - Delete	        View	Edit
    // 1100 = 12 - Delete	 Add 
    // 1101 = 13 - Delete	 Add	        Edit
    // 1110 = 14 - Delete	 Add	 View
    // 1111 = 15 - Delete	 Add	 View	Edit
    public class DAVE {

        private bool _Add;
        private bool _View;
        private bool _Delete;
        private bool _Edit;

        public DAVE() {

        }

        public DAVE(byte perms) {
            // format is DAVE
            //  1000 = 8 = Delete ONLY
            //  0001 = 1 = Edit ONLY
            //  1001 = 9 = Delete and Edit ONLY
            _Delete = ((byte)(perms << 4)) >= 128;
            _Add = ((byte)(perms << 5)) >= 128;
            _View = ((byte)(perms << 6)) >= 128;
            _Edit = ((byte)(perms << 7)) >= 128;

        }

        [UIHint("YesNo")]
        [Display(Name = "Add")]
        public bool Add {
            get { return _Add; }
            set { _Add = value; }
        }

        [UIHint("YesNo")]
        [Display(Name = "View")]
        public bool View {
            get { return _View; }
            set { _View = value; }
        }

        [UIHint("YesNo")]
        [Display(Name = "Archive")]
        public bool Delete {
            get { return _Delete; }
            set { _Delete = value; }
        }

        [UIHint("YesNo")]
        [Display(Name = "Edit")]
        public bool Edit {
            get { return _Edit; }
            set { _Edit = value; }
        }

        public bool AddOrEdit {
            get { return Add || Edit; }
        }

        public bool AddOrEditOrDelete {
            get { return AddOrEdit || Delete; }
        }

        [ScaffoldColumn(false)]
        public byte AsByte {
            get {
                byte value = 0;
                if (Delete) value += 8;
                if (Add) value += 4;
                if (View) value += 2;
                if (Edit) value += 1;
                return value;
            }
        }

        /// <summary>
        ///  This is to adjust that DAVE is not in permission order
        ///  This number will place more "dangerous" permissions at 
        ///  a higher byte value that less "dangerous" permissions
        ///  DELETE is the most dangerous => 8
        ///  EDIT is next most dangerous => 4
        ///  ADD is next most dangerous => 2
        ///  View is least dangerous => 1
        /// </summary>
        private byte Adjusted {
            get {
                byte value = 0;
                if (Delete) value += 8;
                if (Edit) value += 4;
                if (Add) value += 2;
                if (View) value += 1;
                return value;
            }
        }

        public void SetFalse() {
            DAVE tempRolePerm = new DAVE();
            tempRolePerm.Delete = false;
            tempRolePerm.Add = false;
            tempRolePerm.View = false;
            tempRolePerm.Edit = false;
        }

        public override string ToString() {
            return String.Format("Level:{4} | D:{0} A:{1} V:{2} E:{3} | Value: {5}",
                Delete ? "T" : "F", Add ? "T" : "F", View ? "T" : "F", Edit ? "T" : "F",
                Adjusted, AsByte);
        }
        #region Operator Overloads
        public static bool operator >(DAVE d1, DAVE d2) {
            return d1.Adjusted > d2.Adjusted;
        }
        public static bool operator <(DAVE d1, DAVE d2) {
            return d1.Adjusted < d2.Adjusted;
        }
        public static bool operator >=(DAVE d1, DAVE d2) {
            return d1.Adjusted >= d2.Adjusted;
        }
        public static bool operator <=(DAVE d1, DAVE d2) {
            return d1.Adjusted <= d2.Adjusted;
        }
        #endregion
    }
}