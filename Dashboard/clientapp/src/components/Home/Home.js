

export default {
    name: 'home',
    components: {
      
    },
    created() {
       
        //this.getStatistics();
    },
    data() {
        return {
            statistics : {}
        };
    },
    methods: {
        getStatistics() {
            this.$blockUI.Start();
            this.$http.GetStatistics().then(response => {

                this.statistics = response.data;
                this.$blockUI.Stop();
                
            }).catch(e => {
                alert(JSON.stringify(e.message))
            })
        }
    }    
}
