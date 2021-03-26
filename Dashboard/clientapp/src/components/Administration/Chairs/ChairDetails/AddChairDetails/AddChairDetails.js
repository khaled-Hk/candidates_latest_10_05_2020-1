import moment from 'moment';
export default {
    name: 'AddChairDetails',    
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
            Constituencies: [],
            ConstituencyDetails: [],
            ChairsDetails: [],
            regions: [],
            RegionId:'',


            ruleForm: {
                ConstituencyId:'',
                GeneralChairs: '',
                PrivateChairs: '',
                RelativeChairs: '',


                ConstituencyDetailId: '',
                ChairId: '',


            },
            rules: {
                ConstituencyId: [
                    { required: true, message: 'الرجاء إختيار الدائرة الرئيسية', trigger: 'blur' },
                ],
                GeneralChairs: [
                    { required: true, message: 'الرجاء إدخال عدد المقاعد العامة', trigger: 'blur' },
                    { required: true, pattern: /^[0-9]*$/, message: 'الرجاء إدخال ارقام فقط', trigger: 'blur' }
                ],
                PrivateChairs: [
                    { required: true, message: 'الرجاء إدخال عدد المقاعد الخاصة', trigger: 'blur' },
                    { required: true, pattern: /^[0-9]*$/, message: 'الرجاء إدخال ارقام فقط', trigger: 'blur' }
                ],
                RelativeChairs: [
                    { required: true, message: 'الرجاء إدخال عدد المقاعد النسبية', trigger: 'blur' },
                    { required: true, pattern: /^[0-9]*$/, message: 'الرجاء إدخال ارقام فقط', trigger: 'blur' }
                ],
            }
        };
    },
    methods: {

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
            this.$http.GetConstituenciesBasedOn(this.RegionId)
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


        GetConstituenciesDetalsChairs() {
            this.$blockUI.Start();
            this.ruleForm.ConstituencyDetailId = '';
            this.ruleForm.ChairId = '';
            this.$http.GetConstituenciesDetalsChairs(this.ruleForm.ConstituencyId)
                .then(response => {
                    this.$blockUI.Stop();
                /* eslint-disable no-debugger */
                    debugger;
                    this.ConstituencyDetails = response.data.constituencyDetail;
                    this.ChairsDetails = response.data.chairsDetails;
                    this.ruleForm.ChairId = this.ChairsDetails.chairId;
                })
                .catch(() => {
                    this.$blockUI.Stop();
                    //this.$notify({
                    //    title: 'حدث خطأ  ',
                    //    dangerouslyUseHTMLString: true,
                    //    type: 'error',
                    //    message: err.response.data.message
                    //});
                });
        },

     
        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.$blockUI.Start();
                    this.$http.AddChairsDetails(this.ruleForm)
                        .then(response => {
                            this.$parent.state = 0;
                            this.$parent.GetChairs();
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
