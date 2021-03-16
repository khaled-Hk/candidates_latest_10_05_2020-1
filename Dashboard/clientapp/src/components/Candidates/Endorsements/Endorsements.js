import moment from 'moment';

export default {
    name: 'Endorsements',
    created() {
        this.candidateId = this.$parent.CandidateId;
        this.getEndorsements( this.pageNo);
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
            candidateId:null,
            endorsements: [],
            pageNo: 1,
            pageSize: 10,
            candidateName: null,
            pages: 0,
            state: 0
        }
    },
    methods: {

        getEndorsements(pageNo)
        {
           

            if (pageNo === undefined) {
                pageNo = 1;
            }
            this.$blockUI.Start();
            this.$http.GetEndorsements(this.candidateId, pageNo, this.pageSize).then((response) => {
                this.$blockUI.Stop();
                this.endorsements = response.data.endorsements;
                this.candidateName = response.data.candidateName;
                this.pages = response.data.count;
                
            }).catch((error) => {
                this.$blockUI.Stop();
                if (error)
                    this.$message({
                        type: 'error',
                        message: error.response.data
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