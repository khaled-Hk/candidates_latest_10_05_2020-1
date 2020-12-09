import AddEntities from './AddEntities/AddEntities.vue'
import AddCandidates from './AddCandidates/AddCandidates.vue'
import AddRepresentatives from './AddRepresentatives/AddRepresentatives.vue'
import Representatives from './Representatives/Representatives.vue'
import AddUsers from './AddUsers/AddUsers.vue'
import Users from './Users/Users.vue'
//import UpdateCandidates from './UpdateCandidates/UpdateCandidates.vue'
import moment from 'moment';
export default {
    name: 'Entities',
    created() {
        this.GetEntities(this.pageNo);
    },
    components: {
        'AddEntities': AddEntities,
        'AddCandidates': AddCandidates,
        'AddRepresentatives': AddRepresentatives,
        'Representatives': Representatives,
        'AddUsers': AddUsers,
        'Users': Users,
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
            Entites: [],

            EnitiesSelectedId:null,

            

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
        AddEntitesComponent() {
            this.state = 1
        },
        UpdateCandidatesComponent(candidateId) {

            this.state = 2;
            this.CandidateId = candidateId;
        },

        AddCandidate(id) {
            this.state = 2;
            this.EnitiesSelectedId = id;
        },

        AddRepresentatives(id) {
            this.state = 3;
            this.EnitiesSelectedId = id;
        },

        Representatives(id) {
            this.state = 4;
            this.EnitiesSelectedId = id;
        },

        AddUsers(id) {
            this.state = 5;
            this.EnitiesSelectedId = id;
        },

        Users(id) {
            this.state = 6;
            this.EnitiesSelectedId = id;
        },



    }
}
