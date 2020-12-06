import moment from 'moment';
export default {
    name: 'AddCandidates',    
    created() {
        this.GetAllProfiles();
    },
    components: {

    },
    filters: {
        moment: function (date) {
            if (date === null) {
                return "فارغ";
            }
           // return moment(date).format('MMMM Do YYYY, h:mm:ss a');
            return moment(date).format('MMMM Do YYYY');
        }
    },
    data() {
        return {  

            state: 0,
            Candidates: [],
            ruleForm: {
                NID:null
            },
            rules: {
                NID:
                    [
                        {
                            required: true,
                            message: "الرجاء إدخال الرقم الوطني",
                            trigger: "blur",
                        },
                        {
                            min: 12,
                            max: 12,
                            message: "يجب ان يكون طول الرقم الوطني 12 الرقم",
                            trigger: "blur",
                        },
                        { required: true, pattern: /^[0-9]*$/, message: 'الرجاء إدخال ارقام فقط', trigger: 'blur' }
                    ],


            }
        };
    },
    methods: {

        GetAllProfiles() {
            this.loading = true;
            this.$http.GetAllProfiles()
                .then(response => {
                    this.loading = false;
                    this.Profiles = response.data.profile;
                })
                .catch((err) => {
                    this.loading = false;
                    return err;
                });
        },
     
        submitForm(formName) {    
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    //AddProfiles
                    this.$blockUI.Start();
                    this.ruleForm.Number = parseInt(this.ruleForm.Number);
                    this.$http.AddEnitity(this.ruleForm)
                        .then(response => {
                            this.$parent.state = 0;
                            this.$parent.GetEntities();
                            this.$blockUI.Stop();
                            this.$notify({
                                title: 'تمت الاضافة بنجاح',
                                dangerouslyUseHTMLString: true,
                                message: '<strong>' + response.data + '</strong>',
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

        resetForm(formName) {
            this.$refs[formName].resetFields();
        },
        Back() {
            this.$parent.state = 0;
        }
       
  
       
    }    
}
