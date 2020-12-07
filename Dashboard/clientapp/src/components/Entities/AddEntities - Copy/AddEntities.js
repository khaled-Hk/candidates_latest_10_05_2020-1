import moment from 'moment';
export default {
    name: 'AddEntities',    
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
            Profiles: [],
            ruleForm: {
                Name: null,
                Number: null,
                Descriptions: null,
                Phone: null,
                Owner: null,
                Email: null,
                Address: null,
                ProfileId: null,
            },
            rules: {
                Name: [
                    { required: true, message: 'الرجاء إدخال اسم الكيان', trigger: 'blur' },
                    { min: 3, max: 200, message: 'لقد اجتزت الطول المحدد للكيان', trigger: 'blur' }
                ],
                Number: [
                    { required: true, message: 'الرجاء إدخال رقم الكيان', trigger: 'blur' },
                    { required: true, pattern: /^[0-9]*$/, message: 'الرجاء إدخال ارقام فقط', trigger: 'blur' }
                ],
                Phone: [
                    {
                        required: true,
                        message: "الرجاء إدخال رقم الهاتف",
                        trigger: "blur",
                    },
                    {
                        min: 9,
                        max: 13,
                        message: "يجب ان يكون طول رقم الهاتف 9 ارقام علي الاقل",
                        trigger: "blur",
                    },
                    { required: true, pattern: /^[0-9]*$/, message: 'الرجاء إدخال ارقام فقط', trigger: 'blur' }
                ],
                Owner: [
                    { required: true, message: 'الرجاء إدخال اسم المسئول', trigger: 'blur' },
                    { min: 3, max: 200, message: 'لقد اجتزت الطول المحدد للمسئول', trigger: 'blur' }
                ],
                Email: [
                    { required: true, message: 'الرجاء إدخال البريد الإلكتروني', trigger: 'blur' },
                    { min: 5, max: 40, message: 'يجب ان يكون طول البريد الإلكتروني من 5 الي 40 الحرف', trigger: 'blur' },
                    { required: true, pattern: /\S+@\S+\.\S+/, message: 'الرجاء إدخال البريد بطريقة الصحيحه', trigger: 'blur' }
                ], 
                
                ProfileId: [
                    { required: true, message: 'الرجاء اختيار الملف الانتخابي', trigger: 'blur' },
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
