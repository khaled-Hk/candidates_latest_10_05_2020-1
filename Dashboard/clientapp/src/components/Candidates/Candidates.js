import AddCandidates from './AddCandidates/AddCandidates.vue';
import UpdateCandidates from './UpdateCandidates/UpdateCandidates.vue';
import AddRepresentatives from './AddRepresentatives/AddRepresentatives.vue';
import Representatives from './Representatives/Representatives.vue';
import AddUsers from './AddUsers/AddUsers.vue';
import Endorsements from './Endorsements/Endorsements.vue';
import Users from './Users/Users.vue'

import moment from 'moment';
export default {
    name: 'Candidates',
    created() {
        this.GetCandidates(this.pageNo);
        this.GetConstituencies();
    },
    components: {
        'add-Candidates': AddCandidates,
        'update-Candidates': UpdateCandidates,
        'AddRepresentatives': AddRepresentatives,
        'Representatives': Representatives,
        'AddUsers': AddUsers,
        'Users': Users,
        'Endorsements': Endorsements
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
            level:0,
            CandidateId: null,
            candidates: [],
            ConstituencyDetails: [],
            SubConstituency:'',

        };
    },
    methods: {
        FilterBy() {
            this.GetCandidates(this.pageNo);
        },

        GetCandidates(pageNo) {
            this.pageNo = pageNo;
            if (this.pageNo === undefined) {
                this.pageNo = 1;
            }
          
            this.loading = true;
            this.$http.GetCandidates(this.pageNo, this.pageSize, this.SubConstituency || 0)
                .then(response => {
                    this.loading = false;
                    this.candidates = response.data.candidates;
                    this.pages = response.data.count;
                })
                .catch((err) => {
                  
                    this.loading = false;
                 
                    
                   
                    this.$notify({
                        title: 'خطأ بعملية جلب المرشحين',
                        dangerouslyUseHTMLString: true,
                        type: 'error',
                        message: err.response.data
                    });
                    this.pages = 0;
                    this.$blockUI.Stop();
                    return err;
                });

        },


        GetConstituencies() {
            this.$blockUI.Start();
            this.$http.GetConstituencyDetails()
                .then(response => {
                    this.$blockUI.Stop();
                    this.ConstituencyDetails = response.data;
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    return err;
                });

        },



        AddCandidatesComponent() {
            this.state = 1
        },
        UpdateCandidatesComponent(object) {

            this.state = 2;
            this.CandidateId = object.candidateId;
            this.level = object.level;
        },
        Navigate(obj) {
            this.state = obj.state;
            this.CandidateId = obj.candidateId;
        }



    }
}
