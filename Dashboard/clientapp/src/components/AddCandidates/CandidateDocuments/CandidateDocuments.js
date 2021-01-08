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
                BirthCertificateDocument: null,
                NidDocument: null,
                FamilyPaper: null,
                AbsenceOfPrecedents:null,
                PaymentReceipt: null,
                Nid:null
            },
            rules: {

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
       
        SelectBirthCertificateDocument(e)
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
                $this.ruleForm.BirthCertificateDocument = reader.result;
                //$this.UploadImage();
            };
            reader.onerror = function ( ) {
                $this.ruleForm.BirthCertificateDocument = null;
            };
            
            reader.readAsDataURL(files[0]);
           
        },
        SelectNidDocument(e) {
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
                $this.ruleForm.NidDocument = reader.result;
                //$this.UploadImage();
            };
            reader.onerror = function () {
                $this.ruleForm.NidDocument = null;
            };
            reader.readAsDataURL(files[0]);
        },
        SelectFamilyPaper(e) {
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
                $this.ruleForm.FamilyPaper = reader.result;
                //$this.UploadImage();
            };
            reader.onerror = function () {
                $this.ruleForm.FamilyPaper = null;
            };
            reader.readAsDataURL(files[0]);
        },
        SelectAbsenceOfPrecedents(e) {
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
                $this.ruleForm.AbsenceOfPrecedents = reader.result;
                //$this.UploadImage();
            };
            reader.onerror = function () {
                $this.ruleForm.AbsenceOfPrecedents = null;
            };
            reader.readAsDataURL(files[0]);
        },
        SelectPaymentReceipt(e) {
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
                $this.ruleForm.PaymentReceipt = reader.result;
              
            };
            reader.onerror = function () {
                $this.ruleForm.PaymentReceipt = null;
            };
            reader.readAsDataURL(files[0]);
        },
        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    //AddProfiles
                    this.ruleForm.Nid = this.$parent.Nid;
                    
                    this.$blockUI.Start();
                    this.$http.UploadCandidateAttachments(this.ruleForm)
                        .then((response) => {
                           // this.$parent.state = 0;
                            this.$blockUI.Stop();
                            this.$parent.level = response.data.level;
                           
                            this.$notify({
                                title: 'تم الرفع بنجاح',
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
       
    }

}