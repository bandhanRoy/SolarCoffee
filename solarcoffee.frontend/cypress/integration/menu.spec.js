context('SideMenu', () => {
    beforeEach(() => {
        cy.visit('http://localhost:4500');
    })
    it('visits the inventory page via the logo', () => {
        cy.get('#imgLogo').click();
        cy.get('#inventory-title').contains('Inventory Dashboard');
    });

    it('visits the customers page via button click', () => {
        cy.get('#menuCustomer').click();
        cy.get('#customersTitle').contains('Manage Customers');
    });

    it('visits the orders page via button click', () => {
        cy.get('#menuInvoice').click();
        cy.get('#invoiceTitle').contains('Create Invoice');
    });

    it('visits the inventory page via button click', () => {
        cy.get('#menuInventory').click();
        cy.get('#inventory-title').contains('Inventory Dashboard');
    });
})