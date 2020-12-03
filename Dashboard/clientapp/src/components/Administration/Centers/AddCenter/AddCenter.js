import moment from 'moment';
export default {
    name: 'AddConstituency',
    created() {
        this.GetAllConstituencyDetails();
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
            constituencyDetails: [],
            offices: [],
            ruleForm: {
                ArabicName: '',
                EnglishName: '',
                ConstituencDetailId: null,
                OfficeId: null,
                Description: null,
                Longitude: 0.0,
                Latitude: 0.0,
                Stations: []
               
            },
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
                    this.$http.AddCenter(this.ruleForm)
                        .then(response => {
                           // this.$parent.GetConstituencies();
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
        GetAllOffices() {
            this.$blockUI.Start();
            this.$http.GetAllOffices()
                .then(response => {


                    this.$blockUI.Stop();
                    this.offices = response.data.offices;

                })

        },
        GetAllConstituencyDetails() {

            this.$blockUI.Start();
            this.$http.GetConstituencyDetails()
                .then(response => {

                    this.$blockUI.Stop();
                    this.constituencyDetails = response.data.constituencyDetails;

                })


        },
        addStation(index) {
            let station = this.ruleForm.Stations[index]
           
            if (station) {
                station.lastRow = false
            }
               
            this.ruleForm.Stations.push({ ArabicName: null, EnglishName: null, Description: null, lastRow:true});
        },
        removeStations()
        {
            this.ruleForm.Stations = []
        },
        deleteStation(index) {
            this.ruleForm.Stations.splice(index,1)
        },
        resetForm(formName) {
            this.$refs[formName].resetFields();
        },
        Back() {
            this.$parent.state = 0;
        }



    }
}
