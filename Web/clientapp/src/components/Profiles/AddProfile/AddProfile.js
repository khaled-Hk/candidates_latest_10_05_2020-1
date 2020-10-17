import moment from 'moment';
export default {
    name: 'AddProfile',    
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
                name: '',
                Description: '',
                ProfileType:'',
                StartDate: '',
                EndDate: '',
            },
            rules: {
                name: [
                    { required: true, message: 'الرجاء إدخال اسم الملف الإنتخابي', trigger: 'blur' },
                    { min: 3, max: 200, message: 'لقد اجتزت الطول المحدد للملف الانتخابي', trigger: 'blur' }
                ],
                StartDate: [
                    { type: 'date', required: true, message: 'الرجاء إختيار تاريخ البداية للملف الانتخابي', trigger: 'change' }
                ],
                EndDate: [
                    { type: 'date', required: true, message: 'الرجاء إختيار تاريخ النهاية للملف الانتخابي', trigger: 'change' }
                ],
                ProfileType: [
                    { type: 'date', required: true, message: 'الرجاء إختيار نوع الانتخابات', trigger: 'change' }
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
                    this.$http.AddProfiles(this.ruleForm)
                        .then(response => {
                            this.$parent.state = 0;
                            this.$parent.GetProfiles();
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
