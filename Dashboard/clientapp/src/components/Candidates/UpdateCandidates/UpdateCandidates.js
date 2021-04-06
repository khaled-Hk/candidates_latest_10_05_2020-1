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
                Level: 0
               

            },
            documentForm: {
                documentId: null,
                selectedAttachment: null,
                attachment:null
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

        getDocument() {
            this.$blockUI.Start();
            this.$http.GetCandidateAttachment({ CandidateId: this.ruleForm.CandidateId, AttachmenttId: this.documentForm.documentId}).then(response => {

                this.$blockUI.Stop();
                this.documentForm.selectedAttachment = response.data;
            }).catch(err => {

                this.$notify({
                    title: 'حدث خطأ',
                    dangerouslyUseHTMLString: true,
                    type: 'error',
                    message: err.response.data
                });
                this.$blockUI.Stop();
            })
        },
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
                                message: '<strong>' + response.data + '</strong>',
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
                    this.constituencies = response.data;


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
                    this.constituencies = response.data;


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
                    this.subConstituencies = response.data;


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
                    this.subConstituencies = response.data;


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
                    this.ruleForm.Level = candidate.levels;
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
        getAttachmentType() {
            switch (this.documentForm.documentId)
            {
                case 1:
                    return "شهادة الميلاد";
                case 2:
                    return "شهادة الرقم الوطني";
                case 3:
                    return "شهادة وضع العائلة";
                case 4:
                    return "شهادة خلوّ من سوابق";
                case 5:
                    return "إيصال الدفع";
                default:
                    return "";
            }
        },
        SelectDocument(e) {
            var files = e.target.files;

            if (files.length <= 0) {
                return;
            }

            if (files[0].type !== 'application/pdf') {
                this.$message({
                    type: 'error',
                    message: 'يجب ان يكون الملف من نوع  PDF'
                });
            }

            var $this = this;
            var reader = new FileReader();
            reader.onload = function () {
                $this.documentForm.attachment = reader.result;
                //$this.UploadImage();
            };
            reader.onerror = function () {
                $this.documentForm.attachment = null;
            };
            reader.readAsDataURL(files[0]);
        },
        updateCandidateAttachments() {
            this.$blockUI.Start();
            this.$http.UpdateCandidateAttachments({ CandidateId: this.ruleForm.CandidateId, AttachmenttId: this.documentForm.documentId, Attachment: this.documentForm.attachment }).then(response => {
                this.documentForm.selectedAttachment  = response.data.path;
                this.$notify({
                    
                    dangerouslyUseHTMLString: true,
                    type: 'success',
                    message: response.data.message
                });
                this.$blockUI.Stop();
            }).catch(err => {

                this.$notify({
                    title: 'حدث خطأ',
                    dangerouslyUseHTMLString: true,
                    type: 'error',
                    message: err.response.data
                });
                this.$blockUI.Stop();
            })
        },
        Back() {
            this.$parent.state = 0;
        }



    }
}
