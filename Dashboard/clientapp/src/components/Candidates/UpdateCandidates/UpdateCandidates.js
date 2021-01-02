import moment from 'moment';
export default {
    name: 'UpdateCandidates',
    created() {
        
        this.GetAllRegions();
        this.GetCandidate();
       
       
        
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
                CandidateId:null,
               
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
                CompetitionType: null,
               

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
                    this.$http.UpdateCandidate(this.ruleForm)
                        .then(response => {
                         

                           
                            this.$blockUI.Stop();
                            this.$notify({
                                title: 'تمت الاضافة بنجاح',
                                dangerouslyUseHTMLString: true,
                                message: '<strong>' + response.data.message + '</strong>',
                                type: 'success'
                            });
                            this.$parent.GetCandidates(this.$parent.pageNo);
                            this.$parent.state = 0


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
        OnLoadGetConstituencies() {
            this.$blockUI.Start();


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
        OnLoadGetAllConstituencyDetails() {


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
        GetCandidate() {
          
            this.$blockUI.Start();
            this.$http.GetCandidate(this.$parent.CandidateId)
                .then(response => {
                    this.$blockUI.Stop();
                    const candidate = response.data.candidate
                    this.ruleForm.CandidateId = candidate.candidateId;
                    this.ruleForm.FirstName = candidate.firstName;
                    this.ruleForm.FatherName = candidate.fatherName;
                    this.ruleForm.GrandFatherName = candidate.grandFatherName;
                    this.ruleForm.SurName = candidate.surName;
                    this.ruleForm.MotherName = candidate.motherName;
                    this.ruleForm.HomePhone = candidate.homePhone;
                    this.ruleForm.Email = candidate.email;
                    this.ruleForm.SubConstituencyId = candidate.subConstituencyId;
                    this.ruleForm.ConstituencyId = candidate.constituencyId;
                    this.ruleForm.BirthDate = candidate.birthDate;
                 
                    this.ruleForm.Qualification = candidate.qualification;
                    this.ruleForm.CompetitionType = candidate.competitionType;
                    this.ruleForm.Gender = candidate.gender;
                    this.ruleForm.RegionId = response.data.regionId;
                    this.OnLoadGetConstituencies();
                    this.OnLoadGetAllConstituencyDetails();
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
        },
        Back() {
            this.$parent.state = 0;
        }



    }
}
