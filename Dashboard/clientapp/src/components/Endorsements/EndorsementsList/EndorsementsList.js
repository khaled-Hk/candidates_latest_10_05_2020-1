import moment from 'moment';
import AddEndorsement from './AddEndorsement/AddEndorsement.vue';

export default {
    name: 'EndorsementsList',
    created() {
        this.candidateId = this.$parent.ruleForm.CandidateId;
        this.getEndorsements(this.pageNo);
    },
    components: {
        'AddEndorsement': AddEndorsement
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

            candidateId: null,
            endorsements: [],
            pageNo: 1,
            pageSize: 10,
            pages:0,
            candidateName: null,
            state: 0


        }
    },

    methods: {


        getEndorsements(pageNo) {


            if (pageNo === undefined) {
                pageNo = 1;
            }
            this.$blockUI.Start();
            this.$http.GetEndorsements(this.candidateId, pageNo, this.pageSize).then((response) => {
                this.$blockUI.Stop();
                this.endorsements = response.data.endorsements;
                this.pages = response.data.count;
                this.candidateName = response.data.candidateName;
                

            }).catch((error) => {
                this.$blockUI.Stop();
                if (error)
                    this.$message({
                        type: 'error',
                        message: error.message
                    });
                this.endorsements = [];
                return error;
            });

        },
        navigate(state)
        {
            this.state = state;
        },
        Back() {
            this.$parent.state = 0;
            this.$parent.ruleForm.Nid = null;
        }
    }

}