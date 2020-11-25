import moment from 'moment';

export default {
    name: 'CompleteRegistration',
    created() {
       this.fetchCandidateData();
    },
    components: {

    },
    data() {
        return {
            candidate: null

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

        fetchCandidateData() {
            


            this.$blockUI.Start();
            this.$http.GetCandidateInfo(this.$parent.Nid)
                .then((response) => {
                  
                    this.$blockUI.Stop();
                    this.candidate = response.data;
                   
                })
                .catch((err) => {
                    this.$blockUI.Stop();
                    this.$notify({
                        title: 'خطأ بعملية الاضافة',
                        dangerouslyUseHTMLString: true,
                        type: 'error',
                        message: err.response.data.message
                    });
                });
        
        },
        

    }

}