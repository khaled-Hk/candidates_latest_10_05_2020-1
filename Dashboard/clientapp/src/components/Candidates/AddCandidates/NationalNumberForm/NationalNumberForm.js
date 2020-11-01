import moment from 'moment';
export default {
    name: 'nationalnumberform',
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
            level: 0,
            ruleForm: {
                nationalId: null
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
                    //AddProfiles
                    
                    this.$blockUI.Start();
                    this.$http.RegisterCandidateLevelOne(this.ruleForm.nationalId)
                        .then(response => {
                            // this.$parent.GetConstituencies();
                            this.$parent.state = 0;
                            this.$blockUI.Stop();
                            this.$parent.level = response.data.level;
                            this.$parent.Nid = response.data.nid;
                            //this.$notify({
                            //    title: 'تمت الاضافة بنجاح',
                            //    dangerouslyUseHTMLString: true,
                            //    message: '<strong>' + response.data.message + '</strong>',
                            //    type: 'success'
                            //});
                        })
                        .catch((err) => {
                            this.$blockUI.Stop();
                            this.$notify({
                                title: 'خطأ بعملية الاضافة',
                                dangerouslyUseHTMLString: true,
                                type: 'error',
                                message: err.response.data.message
                            });
                        });
                }
            });
        },
        resetform(formname) {
            this.$refs[formname].resetfields();
        },
        back() {
            this.$parent.state = 0;
        }



    }
}
