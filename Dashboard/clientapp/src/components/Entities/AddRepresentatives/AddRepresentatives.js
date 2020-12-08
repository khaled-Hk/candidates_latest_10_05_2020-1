import moment from 'moment';
export default {
    name: 'AddRepresentatives',    
    created() {
        this.ruleForm.EntityId = this.$parent.EnitiesSelectedId;
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
                FirstName: null,
                FatherName: null,
                GrandFatherName: null,
                SurName: null,
                NID: null,
                MotherName: null,
                Gender: null,
                BirthDate: null,
                Phone: null,
                HomePhone: null,
                Email: null,
                EntityId: null,
            },
            rules: {
                FirstName: [
                    { required: true, message: 'الرجاء إدخال الاسم', trigger: 'blur' },
                    { min: 3, max: 200, message: 'لقد اجتزت الطول المحدد للاسم', trigger: 'blur' }
                ],
                FatherName: [
                    { required: true, message: 'الرجاء إدخال الاسم', trigger: 'blur' },
                    { min: 3, max: 200, message: 'لقد اجتزت الطول المحدد للاسم', trigger: 'blur' }
                ],
                GrandFatherName: [
                    { required: true, message: 'الرجاء إدخال الاسم', trigger: 'blur' },
                    { min: 3, max: 200, message: 'لقد اجتزت الطول المحدد للاسم', trigger: 'blur' }
                ],
                SurName: [
                    { required: true, message: 'الرجاء إدخال للقب', trigger: 'blur' },
                    { min: 3, max: 200, message: 'لقد اجتزت الطول المحدد للقب', trigger: 'blur' }
                ],
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
                MotherName: [
                    { required: true, message: 'الرجاء إدخال للقب', trigger: 'blur' },
                    { min: 3, max: 200, message: 'لقد اجتزت الطول المحدد للقب', trigger: 'blur' }
                ],
                Gender: [
                    { required: true, message: 'الرجاء إدخال للقب', trigger: 'blur' },
                ],
                BirthDate: [
                    { required: true, message: 'الرجاء إدخال للقب', trigger: 'blur' },
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
                Email: [
                    { required: true, message: 'الرجاء إدخال البريد الإلكتروني', trigger: 'blur' },
                    { min: 5, max: 40, message: 'يجب ان يكون طول البريد الإلكتروني من 5 الي 40 الحرف', trigger: 'blur' },
                    { required: true, pattern: /\S+@\S+\.\S+/, message: 'الرجاء إدخال البريد بطريقة الصحيحه', trigger: 'blur' }
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
                    this.$http.AddRepresentatives(this.ruleForm)
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
