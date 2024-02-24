<template>
    <v-dialog width="600" v-model="$props.dialog">
        <v-card width="400">
            <v-card-item>
                <v-card-title>Control</v-card-title>
                <v-card-subtitle>{{ this.$props.subTitle }}</v-card-subtitle>
            </v-card-item>

            <v-card-text>
                <v-data-table :items="tableContent" :headers="tableHeader" style="background-color: rgba(255,255,255,0);">
                    <template #bottom></template>
                </v-data-table>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn @click="$props.closeDialog">Close</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
import { VDataTable } from 'vuetify/labs/VDataTable'

export default {
    components: {
        VDataTable,
    },
    props: {
        subTitle: {
            type: String,
            default: ""
        },
        dialog: {
            type: Boolean,
            default: false
        },
        closeDialog: {
            type: Function,
            default: () => { }
        }
    },
    computed: {
        tableContent() {
            if (this.$route.query.scene == 'Mars') {
                return MarsTutor
            }
            if (this.$route.query.scene == 'Universe') {
                return UniverseTutor
            }
            if (this.$route.query.scene == 'Earth') {
                return EarthTutor
            }
            return [
                {
                    key: 'w',
                    action: 'move forward'
                },
                {
                    key: 's',
                    action: 'move backward'
                },
                {
                    key: 'a',
                    action: 'rotate left'
                },
                {
                    key: 'd',
                    action: 'rotate right'
                },
            ]
        },
    },
    data() {
        return {
            tableHeader: [
                { title: 'Key', align: 'center', key: 'key' },
                { title: 'Action', align: 'center', key: 'action' },
            ]
        }
    },
    methods: {
        closeDialog() {
            this.dialog = false
        }
    }
}

const MarsTutor = [
    {
        key: 'w',
        action: 'move forward'
    },
    {
        key: 's',
        action: 'move backward'
    },
    {
        key: 'a',
        action: 'rotate left'
    },
    {
        key: 'd',
        action: 'rotate right'
    },
    {
        key: 'left shift',
        action: 'accelerate(energy consume)'
    },
    {
        key: 'left click',
        action: 'move camera'
    }
]
const UniverseTutor = [
    {
        key: 'w',
        action: 'move forward'
    },
    {
        key: 's',
        action: 'move backward'
    },
    {
        key: 'a',
        action: 'rotate left'
    },
    {
        key: 'd',
        action: 'rotate right'
    },
    {
        key: 'left shift',
        action: 'accelerate(energy consume)'
    },
    {
        key: 'left alt',
        action: 'rotate downward'
    },
    {
        key: 'space',
        action: 'rotate upward'
    },
    {
        key: 'left click',
        action: 'move camera'
    },
    {
        key: 'right click',
        action: 'shoot'
    }
]
const EarthTutor = [
    {
        key: 'w',
        action: 'move forward'
    },
    {
        key: 's',
        action: 'move backward'
    },
    {
        key: 'a',
        action: 'rotate left'
    },
    {
        key: 'd',
        action: 'rotate right'
    },
]
</script>

<style scoped></style>