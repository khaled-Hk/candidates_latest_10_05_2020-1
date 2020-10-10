export default {
    name: 'EditUser',    
    created() {

        this.gender = [
            {
                id: 1,
                name: "ذكر"
            },
            {
                id: 2,
                name: 'انتي'
            }

        ];

        this.UserType = [
            {
                id: 1,
                name: "مدير"
            },
            {
                id: 2,
                name: 'مستخدم'
            }

        ];

        this.form.FullName = this.$parent.EditUsersObj.name;
        this.form.LoginName = this.$parent.EditUsersObj.loginName;
        this.form.Phone = this.$parent.EditUsersObj.phone;
        this.form.Email = this.$parent.EditUsersObj.email;
        this.form.Gender = this.$parent.EditUsersObj.gender;
        this.form.DateOfBirth = this.$parent.EditUsersObj.birthDate;
        this.form.UserId = this.$parent.EditUsersObj.userId;
        this.form.UserType = this.$parent.EditUsersObj.userType;



    },
    data() {
       
        return {
            UserType: [],
           
            pageNo: 1,
            pageSize: 10,
            pages: 0,
            isFromSelect: true,
            Users: [],
            state: 0,
            form: {
                HospitalId: 0,
                LoginName: '',
                Phone: '',
                Password: '',
                FullName: '',
                UserType: 0,
                Email: '',
                Gender: 0,
                DateOfBirth: '',                
               
            },
            ConfimPassword: '',

            gender:[],
        
        };
    },
    methods: {
        Back() {
            this.$parent.state = 0;
        },
       
        validPhone: function (Phone) {

            var mobileRegex = /^09[1|2|3|4|5][0-9]{7}$/i;

            return mobileRegex.test(Phone);
        },

        validEmail: function (email) {
            var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return re.test(email);
        },
        validLoginName: function (LoginName) {
            var login = /^[a-zA-Z]{0,40}$/;
            return login.test(LoginName);
        },
        validFullName: function (FullName) {
            var loginArabic = /^[\u0621-\u064A\u0660-\u0669 ]+$/;
            return loginArabic.test(FullName);
        },

     




        Edit(){

            if (!this.form.Email) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال البريد الإلكتروني '
                });
                return;
            } else if (!this.validEmail(this.form.Email)) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال البريد الإلكتروني بطريقه صحيحه '
                });
                return;
            }

            //if (!this.form.Gender) {
              //  this.$message({
                //    type: 'error',
                //    message: 'الرجاء إختيار الجنس '
                //});
                //return;
            //}

            if (!this.form.DateOfBirth) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء إختيار تاريخ الميلاد '
                });
                return;
            }


            if (!this.form.Phone) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء رقم الهاتف '
                });
                return;
            } else if (!this.validPhone(this.form.Phone)) {
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال رقم الهاتف  بطريقه صحيحه '
                });
                return;
            }
        
           
            this.$http.EditUser(this.form)
                .then(response => {
                    
                    this.$message({
                        type: 'info',
                        message: response.data
                    });
                    this.$parent.EditUsersObj = [];
                    this.$parent.getUser(this.pageNo);
                  this.$parent.state = 0;
                 
                })
                .catch((err) => {
                    this.$message({
                        type: 'error',
                        message: err.response.data
                    });
                });
        }




    }    
}
