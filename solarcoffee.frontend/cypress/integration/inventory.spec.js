context('SideMenu', () => {
    beforeEach(() => {
        cy.visit('http://localhost:4500');
    })
    it('is in the inventory page', () => {
        cy.get('#imgLogo').click();
        cy.get('#inventory-title').contains('Inventory Dashboard');
    });

    it('has buttons to add inventory and receive shipment', () => {
        cy.get('#addNewBtn > .solar-button').contains('Add new item');
        cy.get('#recieveShipmentBtn> .solar-button').contains('Recieve Shipment');
    });

    it('has add new modal title when clicked on add new item', () => {
        cy.get('#addNewBtn > .solar-button').click();
        cy.get('#modalTitle').contains('Add New Product');
        cy.get('[aria-label = "Close Modal"] > .solar-button').click();
    })

    it('adding a new product and closing modal before save does not add new product', () => {
        cy.get('#addNewBtn > .solar-button').click();
        cy.get('#isTaxable').click();
        cy.get('#name').clear();
        cy.get('#name').type('Test product', { delay: 80 });
        cy.get('#description').clear();
        cy.get('#description').type('A great new product for sale', { delay: 80 });
        cy.get('#price').clear();
        cy.get('#price').type('120', { delay: 80 });
        cy.get('[aria-label = "Close Modal"] > .solar-button').click();
    });

    it('adding a new product and closing modal on save new new product', () => {
        cy.get('#addNewBtn > .solar-button').click();
        cy.get('#isTaxable').click();
        cy.get('#name').clear();
        cy.get('#name').type('Test product from cy press', { delay: 80 });
        cy.get('#description').clear();
        cy.get('#description').type('A great new product for sale', { delay: 80 });
        cy.get('#price').clear();
        cy.get('#price').type('120', { delay: 80 });
        cy.get('[aria-label = "Save Product"] > .solar-button').click();
        cy.get('table').contains('td', 'Test product from cy press');
    });

    it('archive a product', () => {
        cy.get('#addNewBtn > .solar-button').click();
        cy.get('#isTaxable').click();
        cy.get('#name').clear();
        cy.get('#name').type('Test product from cy press to Archive', { delay: 80 });
        cy.get('#description').clear();
        cy.get('#description').type('A great new product for sale', { delay: 80 });
        cy.get('#price').clear();
        cy.get('#price').type('120', { delay: 80 });
        cy.get('[aria-label = "Save Product"] > .solar-button').click();
        cy.get('#inventory-table > tr > td > div').last().click();
        cy.get('#inventory-table').not().contains('td', 'Test product from cy press to Archive', { delay: 80 });
    });
})