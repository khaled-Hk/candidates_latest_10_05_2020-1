import AddCandidates from './AddCandidates/AddCandidates.vue'
import moment from 'moment';
export default {
    name: 'Candidates',
    created() {
        this.GetCandidates(this.pageNo);
    },
    components: {
        'add-Candidates': AddCandidates,
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
            candidates:[]
          
        };
    },
    methods: {

       

        GetAllConstituencyDetails() {

            this.$blockUI.Start();
            this.$http.GetConstituencyDetails()
                .then(response => {

                    //this.$parent.GetRegions();

                    this.$blockUI.Stop();
                    this.constituencyDetails = response.data.constituencyDetails;

                })


        },
        GetCandidates(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
            this.loading = true;
            this.$http.GetCandidates(this.pageNo, this.pageSize)
                .then(response => {
                    this.loading = false;
                    this.candidates = response.data.candidates;
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

            candidateId
        }



    }
}
