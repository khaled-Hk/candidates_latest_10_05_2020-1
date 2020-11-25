import moment from 'moment';
export default {
    name: 'UpdateCenter',
    created() {
        this.GetConstituencyDetails();
        this.GetCenter();
        this.GetAllOffices();
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
            constituencyDetails: [],
            offices:[],
            ruleForm: {
                ArabicName: '',
                EnglishName: '',
                ConstituencDetailId: null,
                CenterId:null,
                OfficeId: null,
                Description: null,
                Longitude: 0.0,
                Latitude: 0.0
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
                    this.$http.UpdateCenter(this.ruleForm)
                        .then(response => {
                            this.$parent.GetCenters(this.$parent.pageNo);
                            this.$parent.state = 0;
                            // this.$parent.GetRegions();
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
        GetAllOffices() {
            this.$blockUI.Start();
            this.$http.GetAllOffices()
                .then(response => {
                    this.$blockUI.Stop();
                    this.offices = response.data.offices;

                })

        },
        GetConstituencyDetails() {

            this.$blockUI.Start();
            this.$http.GetConstituencyDetails()
                .then(response => {

                    this.$blockUI.Stop();
                    this.constituencyDetails = response.data.constituencyDetails;

                }).catch((err) => {
                    this.$blockUI.Stop();
                    this.$notify({
                        title: 'حدث خطأ',
                        dangerouslyUseHTMLString: true,
                        type: 'error',
                        message: err.response.data.message
                    });
                });


        },
        GetCenter() {
            this.$blockUI.Start();
            this.$http.GetCenter(this.$parent.centerId)
                .then(response => {

                    //this.$parent.GetRegions();

                    this.$blockUI.Stop();
                    let center = response.data.center;
                    this.ruleForm.ArabicName = center.arabicName
                    this.ruleForm.EnglishName = center.englishName
                    this.ruleForm.ConstituencDetailId = center.constituencyDetailId
                    this.ruleForm.Description = center.description
                    this.ruleForm.Longitude = center.longitude
                    this.ruleForm.Latitude = center.latitude
                    this.ruleForm.CenterId = this.$parent.centerId
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
