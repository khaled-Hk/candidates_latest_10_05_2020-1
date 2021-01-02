import moment from 'moment';
export default {
    name: 'AddEndorsement',
    created() {
        this.ruleForm.CandidateId = this.$parent.candidateId;
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

            ruleForm: {
                CandidateId: null,
                Nid: null
            }
            
        }
    },

    methods: {

        addEndorsements() {

            this.$blockUI.Start();
            this.$http.GetEndorsements(this.ruleForm).then((response) => {
                this.$blockUI.Stop();
                this.endorsements = response.data.endorsements;
                this.candidateName = response.data.candidateName;

            }).catch((error) => {
                this.$blockUI.Stop();
                if (error)
                    this.$message({
                        type: 'error',
                        message: error.response.data.message
                    });
                this.endorsements = [];
                return error;
            });

        },

        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                  
                    this.$blockUI.Start();
                    this.$http.AddEndorsement(this.ruleForm)
                        .then(response => {
                           
                            this.$blockUI.Stop();
                            this.$notify({
                                title: response.data.message,
                                dangerouslyUseHTMLString: true,
                                type: 'info',
                               // message: err.response.data.message
                            });
                            this.$parent.getEndorsements(this.$parent.pageNo, this.$parent.pageSize);
                            this.$parent.state = 0;
                          
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
       
        Back() {
            this.$parent.state = 0;
        }
    }

}