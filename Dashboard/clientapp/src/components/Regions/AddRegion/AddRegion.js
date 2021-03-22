import moment from 'moment';
export default {
    name: 'AddRegion',    
    created() {
       
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
            },
            rules: {
                ArabicName: [
                    { required: true, message: 'الرجاء إدخال اسم المنطقة بالعربي', trigger: 'blur' },
                    { min: 3, max: 200, message: 'لقد اجتزت الطول المحدد للمنطقة', trigger: 'blur' }
                ],
                EnglishName: [
                    { required: true, message: 'الرجاء إدخال اسم المنطقة بالانجليزي', trigger: 'blur' },
                    { min: 3, max: 200, message: 'لقد اجتزت الطول المحدد للمنطقة', trigger: 'blur' }
                ],
               
            }
        };
    },
    methods: {
     
        submitForm(formName) {    
            this.$refs[formName].validate((valid) => {
                if (valid) {
                  
                    this.$blockUI.Start();
                    this.$http.AddRegions(this.ruleForm)
                        .then(response => {
                            this.$parent.state = 0;
                            this.$parent.GetRegions();
                            this.$blockUI.Stop();
                            this.$notify({
                                title: 'تمت الاضافة بنجاح',
                                dangerouslyUseHTMLString: true,
                                message: `<strong>' ${response.data }</strong>`,
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
