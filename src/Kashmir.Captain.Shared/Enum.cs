namespace Kashmir.Captain.Shared.Entities
{
	public enum RoleType
	{
		SuperAdmin, // Me
		Admin, // Manage users, listings, approvals, reports.
		Vendor, // Upload their products or manage listings,
		Contractor, // Rent or purchase tools, equipment, and materials
		Customer, // who is buying/taking on rent
		Worker, // who is working 
	}
}