export default {
    name: 'AddUsers',
    created() {
        this.form.CandidateId = this.$parent.CandidateId;
    },
    data() {
        return {
            pageNo: 1,
            pageSize: 10,
            pages: 0,

            UserType: [],

            form: {
                LoginName: '',
                Password: null,
                Name: '',
                Email: '',
                Gender: '',
                Phone: '',
                BirthDate: '',
                UserType: '',
                Image:null,
                CandidateId: null,

            },
            ConfimPassword: '',
        };
    },
    methods: {

        resetform() {
            this.form.LoginName = '';
            this.form.Password = null;
            this.form.Name = '';
            this.form.UserType = '';
            this.form.Email = '';
            this.form.Gender = '';
            this.form.Phone = '';
            this.form.BirthDate = '';
            this.ConfimPassword = '';
            this.EntityId = null;
            this.$parent.state = 0;
        },


        validEmail: function (email) {
            var re = /^(([^<>()\\[\]\\.,;:\s@"]+(\.[^<>()\\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return re.test(email);
        },
        validLoginName: function (LoginName) {
            var login = /^[a-zA-Z]{0,40}$/;
            return login.test(LoginName);
        },
        validName: function (Name) {
            var loginArabic = /^[\u0621-\u064A\u0660-\u0669 ]+$/;
            return loginArabic.test(Name);
        },

        validPassword: function (Password) {

            var PasswordT = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]){8,}.*$/;

            return PasswordT.test(Password);
        },
        validPhone: function (Phone) {

            var mobileRegex = /^09[1|2|3|4|5][0-9]{7}$/i;

            return mobileRegex.test(Phone);
        },
        SelectImage(e) {

            var files = e.target.files;

            if (files.length <= 0) {
                return;
            }

            if (files[0].type !== 'image/jpeg' && files[0].type !== 'image/png') {
                this.$message({
                    type: 'error',
                    message: 'عفوا يجب انت تكون الصورة من نوع JPG ,PNG'
                });
                this.form.Image = null;
                return;
            }

            var $this = this;

            var reader = new FileReader();
            reader.onload = function () {
                $this.form.Image = reader.result;
                //$this.UploadImage();
            };
            reader.onerror = function () {
                $this.form.Image = null;
            };
            reader.readAsDataURL(files[0]);
        },

        Save() {
            this.$blockUI.Start();

            if (!this.form.LoginName) {
                this.$blockUI.Stop();
                this.$message({
                    type: 'error',
                    message: 'الـرجاء إدخال اسم الدخول'
                });
                return;
            } else if (!this.validLoginName(this.form.LoginName)) {
                this.$blockUI.Stop();
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال اسم الدخول بطريقه صحيحه '
                });
                return;
            }

            if (!this.form.Name) {
                this.$blockUI.Stop();
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال الاسم التلاثي '
                });
                return;
            } else if (!this.validName(this.form.Name)) {
                this.$blockUI.Stop();
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال الاسم التلاثي بطريقه صحيحه '
                });
                return;
            }

            if (!this.form.Email) {
                this.$blockUI.Stop();
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال البريد الإلكتروني '
                });
                return;
            }
            else if (!this.validEmail(this.form.Email)) {
                this.$blockUI.Stop();
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال البريد الإلكتروني بطريقه صحيحه '
                });
                return;
            }

            if (!this.form.Gender) {
                this.$blockUI.Stop();
                this.$message({
                    type: 'error',
                    message: 'الرجاء إختيار الجنس '
                });
                return;
            }

            if (!this.form.Phone) {
                this.$blockUI.Stop();
                this.$message({
                    type: 'error',
                    message: 'الرجاء رقم الهاتف '
                });
                return;
            } else if (!this.validPhone(this.form.Phone)) {
                this.$blockUI.Stop();
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال رقم الهاتف  بطريقه صحيحه '
                });
                return;
            }

            if (!this.form.BirthDate) {
                this.$blockUI.Stop();
                this.$message({
                    type: 'error',
                    message: 'الرجاء إختيار تاريخ الميلاد '
                });
                return;
            }

            if (!this.form.Password) {
                this.$blockUI.Stop();
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال الرقم السري '
                });
                return;
            }
            if (this.form.Password.length <= 6) {
                this.$blockUI.Stop();
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال الرقم السري تحتوي علي سته ارقام '
                });
                return;
            }
            if (!this.validPassword(this.form.Password)) {
                this.$blockUI.Stop();
                this.$message({
                    type: 'error',
                    message: 'عـفوا : يجب ان يحتوي الرقم السري علي حروف صغيرة وكبيرة وارقام'
                });
                return;
            }

            if (!this.ConfimPassword) {
                this.$blockUI.Stop();
                this.$message({
                    type: 'error',
                    message: 'الرجاء إدخال تأكيد الرقم السري '
                });
                return;
            }


            if (this.form.Password != this.ConfimPassword) {
                this.$blockUI.Stop();
                this.$message({
                    type: 'error',
                    message: 'الرجاء التأكد من تطابق الرقم السري'
                });
                return;
            }


            this.$http.AddCandidateUser(this.form)
                .then(response => {
                    this.$message({
                        type: 'info',
                        message: response.data.message
                    });
                    this.resetform();
                    this.$blockUI.Stop();
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    this.$message({
                        type: 'error',
                        message: err.response.data.message
                    });
                });
        },


        Back() {
            this.$parent.state = 0;
        }




    }
}
