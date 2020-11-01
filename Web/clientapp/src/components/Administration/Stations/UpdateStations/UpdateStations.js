import moment from 'moment';
export default {
    name: 'UpdateStations',
    created() {

        this.GetStation();
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
                //CenterId: null,
                StationId: null
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
                    
                    this.$http.UpdateStation(this.ruleForm)
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
        GetStation() {
            this.$blockUI.Start();
            this.ruleForm.StationId = this.$parent.stationId;
            this.$http.GetStationBasedOn(this.ruleForm.StationId)
                .then(response => {


                    this.$blockUI.Stop();
                    let station = response.data.station;
                    this.ruleForm.ArabicName = station.arabicName
                    this.ruleForm.EnglishName = station.englishName
                    this.ruleForm.Description = station.description
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
