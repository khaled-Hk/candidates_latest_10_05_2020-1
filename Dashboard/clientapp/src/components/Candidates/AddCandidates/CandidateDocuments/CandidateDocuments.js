import moment from 'moment';

export default {
    name: 'CandidateDocuments',
    created() {
      
    },
    components: {
    
    },
    data() {
        return {
            ruleForm: {
                CVDocument: '',
                NationalCertificateDocument: null,
                FamilyCertificateDocument: null,
                HealthCertificateDocument: null,
                NoCriminalConvictionDocument: null,
                AcademicDegree:'',
                Nid:null
            }
           
        };
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
    methods: {
       
        SelectCVDocument(e)
        {
            // this.fileList = fileList.slice(-3);
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
                $this.ruleForm.CVDocument = reader.result;
                //$this.UploadImage();
            };
            reader.onerror = function () {
                $this.ruleForm.CVDocument = null;
            };
            reader.readAsDataURL(files[0]);
        },
        save() { },
        addAttachment() { },
        submitForm(formName) {
           
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    //AddProfiles
                    this.ruleForm.Nid = this.$parent.Nid 
                    
                    this.$blockUI.Start();
                    this.$http.UploadFiles(this.ruleForm)
                        .then(() => {
                           // this.$parent.state = 0;
                            this.$blockUI.Stop();
                          //  this.$parent.level = response.data.level;
                           
                            //this.$notify({
                            //    title: 'تمت الاضافة بنجاح',
                            //    dangerouslyUseHTMLString: true,
                            //    message: '<strong>' + response.data.message + '</strong>',
                            //    type: 'success'
                            //});
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
       
    }

}