<template>
    <div class="container">
        <div class="row">
            

            <div v-if="EmployeeGet" class="col" id="employeesGet">
                <table class="table table-hover table-striped table-bordered caption-top">
                    <caption>List of employees</caption>
                    <thead class="table-dark">
                        <tr>
                            <th></th>
                            <!--<th>Id</th>-->
                            <th>Name</th>
                            <th>Number of times</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(emp, index) in EmployeeGet">
                            <td><img src="../icon/person-square.svg"></td>
                            <!--<td>{{ emp.id }}</td>-->
                            <td>{{ emp.name }}</td>
                            <td>{{ emp.numOfTimes }}</td>
                            <td v-if="emp.isAvailable === 0" id="employeeStatusHover">
                                <form @submit.prevent="changeStatusEmployee">
                                    <input type="text" :id="emp.id" :name="'employee'+emp.id" :value="emp.id" v-model="employee['employee'+emp.id]" />
                                    <button class="btn"><img src="../icon/exclamation-circle.svg"></button>
                                    <!--<input type="text" id="empid" :value="emp.id" v-model="employee.id" />
                                    <button class="btn"><img src="../icon/exclamation-circle.svg"></button>-->
                                </form>
                            </td>
                            <td v-if="emp.isAvailable === 1" id="employeeStatusHover">
                                <form @submit.prevent="changeStatusEmployee">
                                    <input type="text" :id="empid" :value="emp.id"/>
                                    <button type="submit" class="btn"><img src="../icon/exclamation-circle-fill.svg"></button>
                                </form>
                            </td>
                            
                        </tr>
                    </tbody>
                </table>
            </div>
            <div v-if="EmployeeRoll" class="col" id="employeesPost">
                <table class="table table-bordered caption-top">
                    <caption>Employee for the next task</caption>
                    <thead class="table-dark">
                        <tr>
                            <th></th>
                            <!--<th>Id</th>-->
                            <th>Name</th>
                            <th>Number of times</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-if="EmployeeRoll">
                            <td><img src="../icon/person-workspace.svg" /></td>
                            <!--<td>{{ EmployeeRoll.id }}</td>-->
                            <td>{{ EmployeeRoll.name }}</td>
                            <td>{{ EmployeeRoll.numOfTimes }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>

        </div>

        <div class="row">
            <div id="employeeRoll" class="col">
                <div>
                    <form @submit.prevent="rollEmployee">
                        <button type="submit" class="btn btn-outline-info btn-sm">Who gets to be the "lucky" one?</button>
                    </form>
                </div>
            </div>
        </div>
    </div>

</template>

<script lang="js">
    import { defineComponent } from 'vue';

    export default defineComponent({
        data() {
            return {
                EmployeeGet: null,
                EmployeePost: null,
                EmployeeRoll: null,
                //employee: [],
                employee: {
                    name: "",
                    id: ""
                }
            };
        },
        async created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchEmployee();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchEmployee'
        },
        methods: {
            fetchEmployee(){
                this.EmployeeGet = null;
                this.Employeeloading = true;

                fetch('Employee')
                    .then(r => r.json())
                    .then(json => {
                        this.EmployeeGet = json;
                        this.Employeeloading = false;
                        return;
                    });
            },
            changeStatusEmployee() {
                this.EmployeePost = null;
                //this.EmployeePostLoading = true;

                fetch('Employee/ChangeStatus/', {
                    method: 'POST',
                    headers: {
                        'Content-type': 'application/json'
                    },
                        body: JSON.stringify({ data: this.employee })
                    })
                    .then(r => r.json())
                    .then(json => {
                        this.EmployeePost = json;
                        //this.EmployeePostLoading = false;
                        return;
                    })
                    .catch(err => {
                        err
                    });
            },
            rollEmployee() {
                this.EmployeeRoll = null;
                //this.EmployeePostLoading = true;

                fetch('Employee/Roll', {
                })
                .then(r => r.json())
                .then(json => {
                    this.EmployeeRoll = json;
                    this.fetchEmployee();
                    //this.EmployeePostLoading = false;
                    return;
                })
                .catch(err => {
                    err
                });
            }
        },
    });

</script>
