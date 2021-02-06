 class StoreCustomer {

    constructor(private firstName:string, private lastName:String ) {
    }

    public visits: number = 0;
    private ourName: string | undefined;

    public showName() {
        alert(this.firstName + " " + this.lastName);
        return true;
    }

    set name(val) {
        this.ourName = val;
    }

    get name() {
        return this.ourName;
    }
}
