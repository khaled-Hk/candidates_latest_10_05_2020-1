import moment from 'moment';
import EndorsementsList from './EndorsementsList/EndorsementsList.vue'
export default {
    name: 'Endorsements',
    created() {
      
    },
    components: {
        'EndorsementsList': EndorsementsList
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
                Nid: null,
                
            },
            state: 0
               

        }
    },

    methods: {


        submitForm(formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {

                    this.$blockUI.Start();
                    this.$http.GetEndorsementsByNid(this.ruleForm.Nid)
                        .then(response => {
                            
                            this.$blockUI.Stop();
                          
                            this.ruleForm.CandidateId = response.data.candidateId;
                            this.state = 1;

                        })
                        .catch((err) => {
                            this.$blockUI.Stop();
                            
                            this.$notify({
                                title: 'خطأ',
                                dangerouslyUseHTMLString: true,
                                type: 'error',
                                message: err.message
                            });
                        });
                }
            });
        },

        Back() {
           // this.$parent.state = 0;
        }
    }

}