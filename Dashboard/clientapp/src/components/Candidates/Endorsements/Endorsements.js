import moment from 'moment';
import AddEndorsement from './AddEndorsement/AddEndorsement.vue'
export default {
    name: 'Endorsements',
    created() {
        this.candidateId = this.$parent.CandidateId;
        this.getEndorsements( this.pageNo, this.pageSize);
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
            candidateId:null,
            endorsements: [],
            pageNo: 1,
            pageSize: 10,
            candidateName: null,
            state: 0
        }
    },
    methods: {

        getEndorsements(pageNo, pageSize)
        {
           

            if (pageNo === undefined) {
                pageNo = 1;
            }
            this.$blockUI.Start();
            this.$http.GetEndorsements(this.candidateId, pageNo, pageSize).then((response) => {
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
        navigateEndorsmenents(pageNo) {
            this.getEndorsements(this.$parent.CandidateId, pageNo, this.pageSize)
        },
        Back()
        {
            this.$parent.state = 0;
        },
        navigate(state)
        {
            this.state = state
        }
    }

}