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
                fileList: [],
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
       
        addAttachment(fieldName, fileList)
        {
            // this.fileList = fileList.slice(-3);
            const formData = new FormData();
            formData.append("Nid", this.ruleForm.Nid);
            if (!fileList.length) return;
            Array
                .from(Array(fileList.length).keys())
                .map(x => {
                    formData.append(fieldName, fileList[x], fileList[x].name);
                });
            this.save(formData);
        },
        save(formData) {
            this.ruleForm.Nid = this.$parent.Nid

            this.$blockUI.Start();
            this.$http.UploadFiles(formData)
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
        },
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