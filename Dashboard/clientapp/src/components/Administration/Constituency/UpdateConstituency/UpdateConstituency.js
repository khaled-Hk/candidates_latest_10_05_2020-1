import moment from 'moment';
export default {
    name: 'UpdateConstituency',
    created() {
        this.GetAllRegions();
        this.GetConstituency();
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
            ruleForm: {
                ArabicName: '',
                EnglishName: '',
                RegionId: null,
                ConstituencyId: null
            },
            rules: {

                RegionId: [
                    { required: true, message: 'الرجاء إختيار المنطقة', trigger: 'blur' }
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
                    this.$http.UpdateConstituency(this.ruleForm)
                        .then(response => {
                            this.$parent.GetConstituencies();
                            this.$parent.state = 0;
                            // this.$parent.GetRegions();
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

                    this.$blockUI.Stop();
                    this.regions = response.data;

                }).catch((err) => {
                    this.$blockUI.Stop();
                    this.$notify({
                        title: 'حدث خطأ',
                        dangerouslyUseHTMLString: true,
                        type: 'error',
                        message: err.response.data
                    });
                });
        },
        GetConstituency() {
            this.$blockUI.Start();
            this.$http.GetConstituencyBasedOn(this.$parent.constituencyId)
                .then(response => {

                    //this.$parent.GetRegions();

                    this.$blockUI.Stop();
                    let constituecny = response.data;
                    this.ruleForm.ArabicName = constituecny.arabicName
                    this.ruleForm.EnglishName = constituecny.englishName
                    this.ruleForm.RegionId = constituecny.regionId
                    this.ruleForm.ConstituencyId = this.$parent.constituencyId
                })
            
        },
        resetForm(formName) {
            this.$refs[formName].resetFields();
        },
        Back() {
            this.$parent.state = 0;
        }



    }
}
