import moment from 'moment';
export default {
    name: 'AddConstituency',
    created() {
        this.GetAllRegions();
       
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
            regions: [],
            Constituencies:[],
            ruleForm: {
                ArabicName: '',
                EnglishName: '',
                ConstituencyId: null,
                RegionId: null
            },
            rules: {

                RegionId: [
                    { required: true, message: 'الرجاء إختيار المنطقة', trigger: 'blur' }
                ],
                ConstituencyId: [
                    { required: true, message: 'الرجاء إختيار المنطقة الرئيسية', trigger: 'blur' }
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
                    this.$http.AddConstituencyDetails(this.ruleForm)
                        .then(response => {
                            this.$parent.GetConstituencyDetails();
                            this.$parent.state = 0;
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

        GetAllRegions() {

            this.$blockUI.Start();
            this.$http.GetAllRegions()
                .then(response => {

                    //this.$parent.GetRegions();

                    this.$blockUI.Stop();
                    this.regions = response.data;

                })


        },

        GetAllConstituencies() {
            this.$blockUI.Start();
            this.ruleForm.ConstituencyId = '';
            this.$http.GetConstituenciesBasedOn(this.ruleForm.RegionId)
                .then(response => {
                    this.$blockUI.Stop();
                    this.Constituencies = response.data;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    this.$notify({
                        title: 'حدث خطأ  ',
                        dangerouslyUseHTMLString: true,
                        type: 'error',
                        message: err.response.data
                    });
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
