import moment from 'moment';
export default {
    name: 'Representatives',    
    created() {
        this.ruleForm.EntityId = this.$parent.EnitiesSelectedId;
        this.GetRepresentativesByEntityId(this.$parent.EnitiesSelectedId);
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

            state: 0,
            representatives: [],
            ruleForm: {
                EntityId:null,
            },
        };
    },
    methods: {

        GetRepresentativesByEntityId(id) {
            this.loading = true;
            this.$http.GetRepresentativesByEntityId(id)
                .then(response => {
                    this.loading = false;
                    this.representatives = response.data;
                })
                .catch((err) => {
                    this.loading = false;
                    return err;
                });
        },

        Back() {
            this.$parent.state = 0;
            this.$parent.EnitiesSelectedId = null;
        }
       
  
       
    }    
}
