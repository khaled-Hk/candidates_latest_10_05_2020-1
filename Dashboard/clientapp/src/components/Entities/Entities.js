//import AddCandidates from './AddCandidates/AddCandidates.vue'
//import UpdateCandidates from './UpdateCandidates/UpdateCandidates.vue'
import moment from 'moment';
export default {
    name: 'Entities',
    created() {
        this.GetEntities(this.pageNo);
    },
    components: {
        //'add-Candidates': AddCandidates,
        //'update-Candidates': UpdateCandidates
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
            state: 0,
            pageNo: 1,
            pageSize: 10,
            pages: 0,
            CandidateId: null,
            Entites: []

        };
    },
    methods: {




        GetEntities(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.loading = true;
            this.$http.GetEntities(this.pageNo, this.pageSize)
                .then(response => {
                    this.loading = false;
                    this.Entites = response.data.entites;
                    this.pages = response.data.count;
                })
                .catch((err) => {
                    this.loading = false;
                    //this.$blockUI.Stop();
                    this.pages = 0;
                    return err;
                });

        },
        AddCandidatesComponent() {
            this.state = 1
        },
        UpdateCandidatesComponent(candidateId) {

            this.state = 2;
            this.CandidateId = candidateId;
        }



    }
}
