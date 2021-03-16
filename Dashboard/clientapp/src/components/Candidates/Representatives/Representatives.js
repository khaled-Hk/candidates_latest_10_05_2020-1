import moment from 'moment';

export default {
    name: 'ConstituencyDetails',
    created() {
        this.GetRepresentatives();
    },
    components: {
 
    },
    data() {
        return {
     
            representatives: [],
            state: 0,
            loading: false,

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
        

      
        GetRepresentatives() {
          
            this.loading = true;
            this.$http.GetCandidateRepresentatives(this.$parent.CandidateId)
                .then(response => {
                    this.loading = false;
                    this.representatives = response.data;
             
                })
                .catch((err) => {
                    this.loading = false;
                    //this.$blockUI.Stop();
                   
                    return err;
                });
        },
        Back() {
            this.$parent.state = 0;
        }
        
    }

}