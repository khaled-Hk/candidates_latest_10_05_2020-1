import moment from 'moment';
export default {
    name: 'AddRepresentatives',
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
            constituencyDetails: [],
            ruleForm: {
                Nid: null,
                FirstName: null,
                FatherName: '',
                GrandFatherName: null,
                SurName: null,
                MotherName: null,
                Gender: null,
                Phone:null,
                HomePhone: null,
                BirthDate: null,
                Email: null,
               

            },
            constituencies: [],
            regions: [],
            subConstituencies: [],
            rules: {

                ConstituencDetailId: [
                    { required: true, message: 'الرجاء إختيار المنطقة الفرعية', trigger: 'blur' }
                ],
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
                    //AddProfiles
                    this.$blockUI.Start();
                    this.ruleForm.CandidateId = this.$parent.CandidateId;
                    this.$http.AddCandidateRepresentatives(this.ruleForm)
                        .then(response => {
                            // this.$parent.GetConstituencies();

                            this.$parent.level = response.data.level
                            this.$blockUI.Stop();
                            this.$notify({
                                title: 'تمت الاضافة بنجاح',
                                dangerouslyUseHTMLString: true,
                                message: '<strong>' + response.data.message + '</strong>',
                                type: 'success'
                            });
                            this.$parent.state = 0;
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
        back() {
            this.$parent.state = 0;
        }



    }
}
