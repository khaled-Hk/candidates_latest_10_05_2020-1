import AddCandidates from './AddCandidates/AddCandidates.vue'
import NationalNumberForm from './NationalNumberForm/NationalNumberForm.vue'
import PhoneForm from './PhoneForm/PhoneForm.vue'
import CandidateDocuments from './CandidateDocuments/CandidateDocuments.vue'
import moment from 'moment';
export default {
    name: 'Candidates',
    created() {
        
    },
    components: {
        'add-Candidates': AddCandidates,
        'NationalNumberForm': NationalNumberForm,
        'PhoneForm': PhoneForm,
        'CandidateDocuments': CandidateDocuments
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
            level: 0,
            Nid: 0,
            Phone:'',
          
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
        addStation(index) {
            let station = this.ruleForm.Stations[index]

            if (station) {
                station.lastRow = false
            }

            this.ruleForm.Stations.push({ ArabicName: null, EnglishName: null, Description: null, lastRow: true });
        },
        removeStations() {
            this.ruleForm.Stations = []
        },
        deleteStation(index) {
            this.ruleForm.Stations.splice(index, 1)
        },
        resetForm(formName) {
            this.$refs[formName].resetFields();
        },
        Back() {
            this.$parent.state = 0;
        }



    }
}
