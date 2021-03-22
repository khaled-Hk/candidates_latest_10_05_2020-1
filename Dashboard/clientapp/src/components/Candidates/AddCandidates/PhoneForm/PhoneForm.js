import moment from 'moment';
export default {
    name: 'PhoneForm',
    created() {

    },
    components: {

    },
    filters: {
        moment: function (date) {
            if (date === null) {
                return "فارغ";
            }
            // return moment(date).format('mmmm do yyyy, h:mm:ss a');
            return moment(date).format('mmmm do yyyy');
        }
    },
    data() {
        return {
            level: this.$parent.level,
            ruleForm: {
                Phone: null,
                Nid: null,
                VerifyCode: null,
                isVerifyCodeSent:false

            },
            rules: {


                nationalId: [
                    { required: true, message: 'الرجاء إدخال اسم المنطقة بالعربي', trigger: 'blur' },
                    { min: 3, max: 200, message: 'لقد اجتزت الطول المحدد للمنطقة', trigger: 'blur' }
                ]

            }
        };
    },
    methods: {

        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                   
                  

                    this.$blockUI.Start();
                    this.$http.RegisterCandidateContact(this.ruleForm)
                        .then(response => {
                            this.ruleForm.isVerifyCodeSent = response.data.isVerifyCodeSent;
                         
                            this.$blockUI.Stop();
                            this.$notify({
                              //  title: 'تمت الاضافة بنجاح',
                                dangerouslyUseHTMLString: true,
                                message: '<strong>' + response.data.message + '</strong>',
                                type: 'success'
                            });
                        })
                        .catch((err) => {
                            this.$blockUI.Stop();
                            this.$notify({
                                title: 'خطأ بعملية الاضافة',
                                dangerouslyUseHTMLString: true,
                                type: 'error',
                                message: err.response.data
                            });
                        });
                }
            });
        },
        submitVerifyCode(formName)
        {
            
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    //AddProfiles
                    this.ruleForm.Nid = this.$parent.Nid;
                    this.ruleForm.VerifyCode = parseInt(this.ruleForm.VerifyCode)
                    this.$blockUI.Start();
                    this.$http.VerifyPhone(this.ruleForm)
                        .then(response => {
                            this.$parent.level = response.data.level;
                         
                            this.$blockUI.Stop();
                            this.$notify({
                                //  title: 'تمت الاضافة بنجاح',
                                dangerouslyUseHTMLString: true,
                                message: '<strong>' + response.data.message + '</strong>',
                                type: 'success'
                            });
                        })
                        .catch((err) => {
                            this.$blockUI.Stop();
                            this.$notify({
                                title: 'خطأ بعملية الاضافة',
                                dangerouslyUseHTMLString: true,
                                type: 'error',
                                message: err.response.data
                            });
                        });
                }
            });
        }



    }
}
