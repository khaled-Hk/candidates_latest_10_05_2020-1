import moment from 'moment';
export default {
    name: 'AddStations',
    created() {
       

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
            ruleForm: {
                ArabicName: '',
                EnglishName: '',
                Description: '',
                CenterId: null
            },
            rules: {

              
                ArabicName: [
                    { required: true, message: 'الرجاء إدخال اسم المحطة بالعربي', trigger: 'blur' },
                    { min: 3, max: 200, message: 'لقد اجتزت الطول المحدد للمحطة', trigger: 'blur' }
                ],
                EnglishName: [
                    { required: true, message: 'الرجاء إدخال اسم المحطة بالانجليزي', trigger: 'blur' },
                    { min: 3, max: 200, message: 'لقد اجتزت الطول المحدد للمحطة', trigger: 'blur' }
                ],

            }
        };
    },
    methods: {

        submitForm(formName) {
           
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    //AddProfiles
                    this.$blockUI.Start();
                    this.ruleForm.CenterId = this.$parent.centerId
                    this.$http.CreateStations(this.ruleForm)
                        .then(response => {
                            this.$parent.GetStations(this.$parent.pageNo);
                            this.$parent.state = 0;
                            this.$blockUI.Stop();
                            this.$notify({
                                title: 'تمت الاضافة بنجاح',
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
                                message: err.response.data.message
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
