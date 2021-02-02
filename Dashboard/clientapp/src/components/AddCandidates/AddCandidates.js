import CandidateData from './CandidateData/CandidateData.vue'
import NationalNumberForm from './NationalNumberForm/NationalNumberForm.vue'
import PhoneForm from './PhoneForm/PhoneForm.vue'
import CandidateDocuments from './CandidateDocuments/CandidateDocuments.vue'
import AddRepresentatives from './AddRepresentatives/AddRepresentatives.vue'
import EndorsementsList from './EndorsementsList/EndorsementsList.vue'
import CompleteRegistration from './CompleteRegistration/CompleteRegistration.vue'

import moment from 'moment';
export default {
    name: 'Candidates',
    created() {
        
    },
    components: {
        'candidates-data': CandidateData,
        'NationalNumberForm': NationalNumberForm,
        'PhoneForm': PhoneForm,
        'CandidateDocuments': CandidateDocuments,
        'AddRepresentatives': AddRepresentatives,
        'CompleteRegistration': CompleteRegistration,
        'EndorsementsList': EndorsementsList
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
            Phone: '',
            candidateId:0
          
        };
    },
    methods: {

       
       
       
     
        Back() {
            this.$parent.state = 0;
        }



    }
}
