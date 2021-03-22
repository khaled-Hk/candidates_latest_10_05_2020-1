import moment from 'moment';
export default {
    name: 'AddCandidates',
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
            constituencyDetails: [],
            ruleForm: {
                Nid:null,
                FirstName: null,
                FatherName: '',
                GrandFatherName: null,
                SurName: null,
                MotherName: null,
                Gender: null,
                HomePhone: null,
                BirthDate: null,
                Email: null,
                Qualification: null,
                ConstituencyId: null,
                RegionId: null,
                SubConstituencyId: null,
                CompetitionType:null,
               
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
                    this.ruleForm.Nid = this.$parent.Nid;
                    this.$http.UploadCandidateData(this.ruleForm)
                        .then(response => {
                            // this.$parent.GetConstituencies();
                          
                            this.$parent.level = response.data.level
                            this.$blockUI.Stop();
                            this.$notify({
                                title: 'تمت الاضافة بنجاح',
                                dangerouslyUseHTMLString: true,
                                message: `<strong>' ${response.data.message}</strong>`,
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
            this.ruleForm.RegionId = null;
            this.ruleForm.ConstituencyId = null;
            this.ruleForm.SubConstituencyId = null
          
            this.$http.GetAllRegions()
                .then(response => {

                    this.regions = response.data;
                    this.$blockUI.Stop();
                })
                .catch((err) => {
                    //  this.loading = false;
                    //this.$blockUI.Stop();
                    this.$blockUI.Stop();
                    return err;
                });
        },

        GetConstituencies() {
            this.$blockUI.Start();
            
            this.ruleForm.ConstituencyId = null;
            this.ruleForm.SubConstituencyId = null
            
            this.$http.GetAConstituencyBasedOn(this.ruleForm.RegionId)
                .then(response => {
                    this.$blockUI.Stop();
                    this.constituencies = response.data.constituency;


                })
                .catch((err) => {
                    //  this.loading = false;
                    //this.$blockUI.Stop();
                    this.$blockUI.Stop();
                    return err;
                });
        },
        GetAllConstituencyDetails() {
          
            this.ruleForm.SubConstituencyId = null

            this.$blockUI.Start();
            this.$http.GetAllConstituencyDetailsBasedOn(this.ruleForm.ConstituencyId)
                .then(response => {
                    this.$blockUI.Stop();
                    this.subConstituencies = response.data.constituencyDetails;


                })
                .catch((err) => {
                    //  this.loading = false;
                    //this.$blockUI.Stop();
                    this.$blockUI.Stop();
                    return err;
                });
        },
       
        resetForm(formName) {
            this.$refs[formName].resetFields();
        }



    }
}
