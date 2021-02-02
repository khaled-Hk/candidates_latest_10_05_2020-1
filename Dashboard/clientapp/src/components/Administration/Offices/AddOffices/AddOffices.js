import moment from 'moment';
export default {
    name: 'AddProfile',    
    created() {

        this.GetAllProfiles(); 
        this.GetAllBranches();  
       
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
            Profiles: [],
            Branches: [],
            ruleForm: {
                ArabicName: '',
                EnglishName: '',
                Description: '',
                //BranchId:'',
                //ProfileId: '',
            },
            rules: {
                ArabicName: [
                    { required: true, message: 'الرجاء إدخال اسم المكتب الانتخابي', trigger: 'blur' },
                    { min: 3, max: 200, message: 'لقد اجتزت الطول المحدد للملف الانتخابي', trigger: 'blur' },
                    { required: true, pattern: /[\u0600-\u06FF]/, message: 'الرجاء إدخال حروف العربية فقط', trigger: 'blur' }
                ],
                EnglishName: [
                    { required: true, message: 'الرجاء إدخال اسم المكتب الانتخابي', trigger: 'blur' },
                    { min: 3, max: 200, message: 'لقد اجتزت الطول المحدد للملف الانتخابي', trigger: 'blur' },
                    { required: true, pattern: /^[a-zA-Z ]+$/, message: 'الرجاء إدخال حروف انجليزية فقط', trigger: 'blur' }
                ],
                //BranchId: [
                //    { required: true, message: 'الرجاء إختيار الفرع', trigger: 'blur' },
                //],
                //ProfileId: [
                //    { required: true, message: 'الرجاء إختيار الملف الانتخابي', trigger: 'blur' },
                //],
            }
        };
    },
    methods: {

        GetAllBranches() {
            this.loading = true;
            this.$http.GetAllBranches()
                .then(response => {
                    this.loading = false;
                    this.Branches = response.data.branches;
                })
                .catch((err) => {
                    this.loading = false;
                    return err;
                });
        },

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
                    this.$blockUI.Start();
                    this.$http.AddOffice(this.ruleForm)
                        .then(response => {
                            this.$parent.state = 0;
                            this.$parent.GetOffice();
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
