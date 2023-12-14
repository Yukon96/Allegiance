<template>
    <h1 id="top-welcome">Edit Welcome Page Here Administrator!</h1>
    <div class="holder">
        <div class="welcome-container">
            <form v-on:submit.prevent="saveView()">
                <div class="welcome-header-image-container">
                    <label id="label" for="welcome-header">Header</label>
                    <input id="field" type="text" name="welcome-header" v-model="home.header">
                    <label for="fileInput" id="label">Img URL</label>
                    <input id="field" type="text" name="welcome-image" v-model="home.image" />
                </div>
                <div class="welcome-name-body-container">
                    <label id="label" for="welcome-name">Name</label>
                    <input id="field" type="text" name="welcome-name" v-model="home.name">
                    <label id="label" for="welcome-body">Body</label>
                    <input id="welcome-body" type="text" name="welcome-body" v-model="home.body">
                </div>
                <div class="save-and-submit">
                    <input v-bind:value="'Save'" type="submit" />
                    <input v-bind:value="'Save and Apply'" type="submit"
                        v-on:click.prevent="saveAndApplyDisplay('apply')" />
                </div>
            </form>
            <div class="please-select-one">
                <form v-on:submit="changeHomeDisplay(selectedView)">
                    <select v-on:click="watchIdChange($event)" v-model="selectedView">
                        <option disabled value="">Select From Formats</option>
                        <option :value="view.viewId" :id="view.name" v-bind:key="view.viewId" v-for="view in homeviews">{{
                            view.name
                        }}</option>
                    </select>
                    <input v-bind:value="'Apply'" type="submit" />
                    <input v-bind:value="'Delete'" type="submit" v-on:click.prevent="deleteView()" />
                </form>
            </div>
        </div>
    </div>
</template>
<script>
import HomeViewService from '../../services/HomeViewService';
export default {
    created() {
        this.getSavedViews();
    },
    data() {
        return {
            home: {
                body: '',
                header: '',
                image: '',
                name: ''
            },
            homeviews: [],
            selectedView: "",
            watchId: 0,
        }
    },
    methods: {
        watchIdChange(event) {
            this.watchId = event.target.value
        },
        deleteView() {
            let filter = this.selectedView;
            HomeViewService.deleteView(this.selectedView)
                .catch((error) => {
                    const response = error.response;
                    this.registrationErrors = true;
                    if (response.status === 400) {
                        this.registrationErrorMsg = 'Bad Request: Validation Errors';
                    }
                }).finally(this.getSavedViews)
        },
        saveAndApplyDisplay(apply) {
            this.saveView(apply)
            HomeViewService.putDisplayView(-1)
                .then(this.home = {})
                .catch((error) => {
                    const response = error.response;
                    this.registrationErrors = true;
                    if (response.status === 400) {
                        this.registrationErrorMsg = 'Bad Request: Validation Errors';
                    }
                })
        },
        changeHomeDisplay(save) {
            let id = parseInt(this.watchId);

            HomeViewService.putDisplayView(id)
                .then((response) => {
                    if (response.status == 200) {
                        this.home = {}
                    }
                })
                .catch((error) => {
                    const response = error.response;
                    this.registrationErrors = true;
                    if (response.status == 400) {
                        this.registrationErrorMsg = 'Bad Request: Validation Errors';
                    }
                })
        },
        getSavedViews() {
            this.homeviews = [];
            HomeViewService
                .getAllViews()
                .then((response) => {
                    if (response.status == 200) {
                        response.data.forEach(element => this.addToHomeViews(element))
                    }
                })
                .catch((error) => {
                    const response = error.response;
                    this.registrationErrors = true;
                    if (response.status == 400) {
                        this.registrationErrorMsg = 'Bad Request: Validation Errors';
                    }
                })
        },
        addToHomeViews(element) {
            this.homeviews.push(element)
        },
        saveView() {
            HomeViewService
                .postView(this.home)
                .then((response) => {
                    if (response.status == 200) {
                        this.homeviews.push(response.data);
                        this.home = {};
                    }
                })
                .catch((error) => {
                    const response = error.response;
                    this.registrationErrors = true;
                    if (response.status == 400) {
                        this.registrationErrorMsg = 'Bad Request: Validation Errors';
                    }
                })
        }
    },
}
</script>

<style scoped>
.holder {
    width: 100vw;
}

.welcome-container {
    height: fit-content;
    margin-left: auto;
    margin-right: auto;
    width: 50%;
    color: black;
    border-radius: 2.5vh;
    background-color: aliceblue;
    border-color: black;
    border-style: solid;
    text-align: center;
    grid-area: welcome-container;
    display: grid;
    grid-template-columns: 1fr;
    align-items: center;
    justify-items: center;
    justify-content: center;
    grid-template-areas:
        "welcome-header-body-container"
        "welcome-name-image-container"
        "please-select-one"
        "save-and-submit";
}

.welcome-header-image-container {
    width: 100%;
    margin: auto;
    margin-top: 10px;
    margin-bottom: 10px;
    display: flex;
    text-align: center;
    align-items: center;
    justify-items: center;
    grid-column-gap: 10px;
    grid-area: welcome-header-image-container;
}

#label {
    width: 20%;
}

#field {
    width: 30%;
}

.welcome-name-body-container {
    margin: auto;   
    display: flex;
    grid-area: welcome-name-body-container;
    margin-bottom: 10px;
    grid-column-gap: 10px;
    text-align: center;
    align-items: center;
    justify-items: center;
}

.please-select-one {
    margin: auto;
    grid-area: please-select-one;
}

.save-and-submit {
    margin: auto;
    grid-area: save-and-submit;
    margin-bottom: 10px;
}
</style>