namespace OPC
{
    using System;

    public class ErrorDescriptions
    {
        public static string GetErrorDescription(int hr)
        {
            string errorDescription = new OPCErrorCodes().GetErrorDescription((long) hr);
            if (errorDescription == null)
            {
                errorDescription = OPCErrorCodes.Win32Errors.GetText(hr);
                if (errorDescription == null)
                {
                    errorDescription = "";
                }
            }
            return errorDescription;
        }

        internal class OPCErrorCodes
        {
            private ErrorDef[] Errors = new ErrorDef[] { 
                new ErrorDef(0x4000d, "", "The server does not support the requested rate but will use the closest available rate."), new ErrorDef(0x4000e, "", "A value passed to WRITE was accepted but the output was clamped."), new ErrorDef(0x4000f, "", "The operation cannot be performed because the object is being referenced."), new ErrorDef(0x80000001, "", "Not implemented."), new ErrorDef(0x80000002, "", "The operation could not complete due to memory limitations."), new ErrorDef(0x80004002, "", "No such interface supported."), new ErrorDef(0x80004004, "", "Operation aborted."), new ErrorDef(0x80004005, "", "The operation failed."), new ErrorDef(0x80010105, "", "The server threw an exception."), new ErrorDef(0x80010108, "", "The RPC connection is disconnected."), new ErrorDef(0x8001010e, "", "Wrong thread makes COM call. Maybe callback to STA client."), new ErrorDef(0x80010119, "", "Security must be initialized before any interfaces are marshalled or unmarshalled."), new ErrorDef(0x80020005, "DISP_E_TYPEMISMATCH", "Wrong data type."), new ErrorDef(0x8002000a, "DISP_E_OVERFLOW", "Probably means that an INT16 or FLOAT type was requested for an INT32 resp. DOUBLE value"), new ErrorDef(0x80040000, "", "Cannot Unadvise - there is no existing connection."), new ErrorDef(0x80040001, "", "Advise limit exceeded for this object."), 
                new ErrorDef(0x80040005, "OLE_NOT_RUNNING", "Probably means the OPC server was shutdown and DCOM was unable to start it."), new ErrorDef(0x80040064, "", "Invalid or unregistered Format specified in FORMATETC."), new ErrorDef(0x80040112, "", "Appropriate license not found. E.g. No RSLinx Activation is on the computer."), new ErrorDef(0x80040154, "", "Class Not Registered."), new ErrorDef(0x800401f0, "", "CoInitialize has not been called for this thread."), new ErrorDef(0x800401f1, "", "CoInitialize has already been called."), new ErrorDef(0x800401f2, "", "Class of object cannot be determined."), new ErrorDef(0x800401f3, "", "Invalid class string."), new ErrorDef(0x800401f4, "", "Invalid interface string."), new ErrorDef(0x800401f5, "", "Application not found."), new ErrorDef(0x800401f6, "", "Application cannot be run more than once."), new ErrorDef(0x800401f7, "", "Some error in application program."), new ErrorDef(0x800401f8, "", "DLL for class not found."), new ErrorDef(0x800401f9, "", "Error in the DLL."), new ErrorDef(0x800401fa, "", "Wrong OS or OS version for application."), new ErrorDef(0x800401fb, "", "Object is not registered."), 
                new ErrorDef(0x800401fc, "", "Object is already registered."), new ErrorDef(0x800401fd, "", "Object is not connected to server."), new ErrorDef(0x800401fe, "", "Application was launched but it didn't register a class factory."), new ErrorDef(0x800401ff, "", "Object has been released."), new ErrorDef(0x80040200, "", "Unable to impersonate DCOM client"), new ErrorDef(0x80040201, "", "Unable to obtain server's security context"), new ErrorDef(0x80040202, "", "Unable to open the access token of the current thread"), new ErrorDef(0x80040203, "", "Unable to obtain user info from an access token"), new ErrorDef(0x80040204, "", "The client who called IAccessControl::IsAccessPermitted was the trustee provided tot he method"), new ErrorDef(0x80040205, "", "Unable to obtain the client's security blanket"), new ErrorDef(0x80040206, "", "Unable to set a discretionary ACL into a security descriptor"), new ErrorDef(0x80040207, "", "The system function, AccessCheck, returned false"), new ErrorDef(0x80040208, "", "Either NetAccessDel or NetAccessAdd returned an error code."), new ErrorDef(0x80040209, "", @"One of the trustee strings provided by the user did not conform to the <Domain>\<Name> syntax and it was not the '*' string"), new ErrorDef(0x8004020a, "", "One of the security identifiers provided by the user was invalid"), new ErrorDef(0x8004020b, "", "Unable to convert a wide character trustee string to a multibyte trustee string"), 
                new ErrorDef(0x8004020c, "", "Unable to find a security identifier that corresponds to a trustee string provided by the user"), new ErrorDef(0x8004020d, "", "The system function, LookupAccountSID, failed"), new ErrorDef(0x8004020e, "", "Unable to find a trustee name that corresponds to a security identifier provided by the user"), new ErrorDef(0x8004020f, "", "The system function, LookupAccountName, failed"), new ErrorDef(0x80040210, "", "Unable to set or reset a serialization handle"), new ErrorDef(0x80040211, "", "Unable to obtain the Windows directory"), new ErrorDef(0x80040212, "", "Path too long"), new ErrorDef(0x80040213, "", "Unable to generate a uuid."), new ErrorDef(0x80040214, "", "Unable to create file"), new ErrorDef(0x80040215, "", "Unable to close a serialization handle or a file handle."), new ErrorDef(0x80040216, "", "The number of ACEs in an ACL exceeds the system limit"), new ErrorDef(0x80040217, "", " Not all the DENY_ACCESS ACEs are arranged in front of the GRANT_ACCESS ACEs in the stream"), new ErrorDef(0x80040218, "", "The version of ACL format in the stream is not supported by this implementation of IAccessControl"), new ErrorDef(0x80040219, "", "Unable to open the access token of the server process"), new ErrorDef(0x8004021a, "", "Unable to decode the ACL in the stream provided by the user"), new ErrorDef(0x8004021b, "", "The COM IAccessControl object is not initialized"), 
                new ErrorDef(0x8004f800, "", "No Server identification specified in Client Handle."), new ErrorDef(0x8004f801, "", "Unknown Server identification in Client Handle."), new ErrorDef(0x8004f802, "", "The Internal Simulation does not support subscriptions/item properties."), new ErrorDef(0x8004f803, "", "The group could not be created in the OPC server."), new ErrorDef(0x8004ffff, "", "An error occurred that is not known to the OPC XML Service."), new ErrorDef(0x80070001, "", "Incorrect function.."), new ErrorDef(0x80070002, "", "The system cannot find the file specified."), new ErrorDef(0x80070003, "", "The system cannot find the path specified."), new ErrorDef(0x80070004, "", "The system cannot open the file."), new ErrorDef(0x80070005, "", "Access denied."), new ErrorDef(0x80070006, "", "The handle is invalid."), new ErrorDef(0x80070007, "", "The storage control blocks were destroyed. "), new ErrorDef(0x80070008, "", "Not enough storage is available to process this command."), new ErrorDef(0x80070009, "", "The storage control block address is invalid."), new ErrorDef(0x8007000a, "", "The environment is incorrect."), new ErrorDef(0x8007000b, "", "An attempt was made to load a program with an incorrect format."), 
                new ErrorDef(0x8007000c, "", "The access code is invalid."), new ErrorDef(0x8007000d, "", "The data is invalid."), new ErrorDef(0x8007000e, "", "Out of memory."), new ErrorDef(0x80070057, "", "The value of one or more parameters was not valid."), new ErrorDef(0x800700c1, "ERROR_BAD_EXE_FORMAT", "The file is corrupted or was renamed from another file type."), new ErrorDef(0x80070424, "", "The specified service does not exist as an installed service."), new ErrorDef(0x800706a4, "RPC_S_INVALID_STRING_BINDING", "The string binding is invalid."), new ErrorDef(0x800706a5, "RPC_S_WRONG_KIND_OF_BINDING", "The binding handle is the incorrect type."), new ErrorDef(0x800706a6, "RPC_S_INVALID_BINDING", "The binding handle is invalid."), new ErrorDef(0x800706a7, "RPC_S_PROTSEQ_NOT_SUPPORTED", "The RPC protocol sequence is not supported."), new ErrorDef(0x800706a8, "RPC_S_INVALID_RPC_PROTSEQ", "The RPC protocol sequence is invalid."), new ErrorDef(0x800706a9, "RPC_S_INVALID_STRING_UUID", "The string UUID is invalid."), new ErrorDef(0x800706aa, "RPC_S_INVALID_ENDPOINT_FORMAT", "The endpoint format is invalid."), new ErrorDef(0x800706ab, "RPC_S_INVALID_NET_ADDR", "The network address is invalid."), new ErrorDef(0x800706ac, "RPC_S_NO_ENDPOINT_FOUND", "No endpoint was found."), new ErrorDef(0x800706ad, "RPC_S_INVALID_TIMEOUT", "The timeout value is invalid."), 
                new ErrorDef(0x800706ae, "RPC_S_OBJECT_NOT_FOUND", "The object UUID was not found."), new ErrorDef(0x800706af, "RPC_S_ALREADY_REGISTERED", "The object UUID already registered."), new ErrorDef(0x800706b0, "RPC_S_TYPE_ALREADY_REGISTERED", "The type UUID is already registered."), new ErrorDef(0x800706b1, "RPC_S_ALREADY_LISTENING", "The server is already listening."), new ErrorDef(0x800706b2, "RPC_S_NO_PROTSEQS_REGISTERED", "No protocol sequences were registered."), new ErrorDef(0x800706b3, "RPC_S_NOT_LISTENING", "The server is not listening."), new ErrorDef(0x800706b4, "RPC_S_UNKNOWN_MGR_TYPE", "The manager type is unknown."), new ErrorDef(0x800706b5, "RPC_S_UNKNOWN_IF", "The interface is unknown."), new ErrorDef(0x800706b6, "RPC_S_NO_BINDINGS", "There are no bindings."), new ErrorDef(0x800706b7, "RPC_S_NO_PROTSEQS", "There are no protocol sequences."), new ErrorDef(0x800706b8, "RPC_S_CANT_CREATE_ENDPOINT", "The endpoint cannot be created."), new ErrorDef(0x800706b9, "RPC_S_OUT_OF_RESOURCES", "Not enough resources are available to complete this operation."), new ErrorDef(0x800706ba, "RPC_S_SERVER_UNAVAILABLE", "The server is unavailable."), new ErrorDef(0x800706bb, "RPC_S_SERVER_TOO_BUSY", "The server is too busy to complete this operation."), new ErrorDef(0x800706bc, "RPC_S_INVALID_NETWORK_OPTIONS", "The network options are invalid."), new ErrorDef(0x800706bd, "RPC_S_NO_CALL_ACTIVE", "There is not a remote procedure call active in this thread."), 
                new ErrorDef(0x800706be, "RPC_S_CALL_FAILED", "The remote procedure call failed."), new ErrorDef(0x800706bf, "RPC_S_CALL_FAILED_DNE", "The remote procedure call failed and did not execute."), new ErrorDef(0x800706c0, "RPC_S_PROTOCOL_ERROR", "An RPC protocol error occurred."), new ErrorDef(0x800706c2, "RPC_S_UNSUPPORTED_TRANS_SYN", "The transfer syntax is not supported by the server."), new ErrorDef(0x800706c3, "RPC_S_SERVER_OUT_OF_MEMORY", "The server has insufficient memory to complete this operation."), new ErrorDef(0x800706c4, "RPC_S_UNSUPPORTED_TYPE", "The type UUID is not supported."), new ErrorDef(0x800706c5, "RPC_S_INVALID_TAG", "The tag is invalid."), new ErrorDef(0x800706c6, "RPC_S_INVALID_BOUND", "The array bounds are invalid."), new ErrorDef(0x800706c7, "RPC_S_NO_ENTRY_NAME", "The binding does not contain an entry name."), new ErrorDef(0x800706c8, "RPC_S_INVALID_NAME_SYNTAX", "The name syntax is invalid."), new ErrorDef(0x800706c9, "RPC_S_UNSUPPORTED_NAME_SYNTAX", "The name syntax is not supported."), new ErrorDef(0x800706cb, "RPC_S_UUID_NO_ADDRESS", "No network address is available to use to construct a UUID."), new ErrorDef(0x800706cc, "RPC_S_DUPLICATE_ENDPOINT", "The endpoint is a duplicate."), new ErrorDef(0x800706cd, "RPC_S_UNKNOWN_AUTHN_TYPE", "The authentication type is unknown."), new ErrorDef(0x800706ce, "RPC_S_MAX_CALLS_TOO_SMALL", "The maximum number of calls is too small."), new ErrorDef(0x800706cf, "RPC_S_STRING_TOO_LONG", "The string is too long."), 
                new ErrorDef(0x800706d0, "RPC_S_PROTSEQ_NOT_FOUND", "The RPC protocol sequence was not found."), new ErrorDef(0x800706d1, "RPC_S_PROCNUM_OUT_OF_RANGE", "The procedure number is out of range."), new ErrorDef(0x800706d2, "RPC_S_BINDING_HAS_NO_AUTH", "The binding does not contain any authentication information."), new ErrorDef(0x800706d3, "RPC_S_UNKNOWN_AUTHN_SERVICE", "The authentication service is unknown."), new ErrorDef(0x800706d4, "RPC_S_UNKNOWN_AUTHN_LEVEL", "The authentication level is unknown."), new ErrorDef(0x800706d5, "RPC_S_INVALID_AUTH_IDENTITY", "The security context is invalid."), new ErrorDef(0x800706f4, "", "NULL is passed to COM as a reference pointer"), new ErrorDef(0x80080001, "", "Attempt to create a class object failed."), new ErrorDef(0x80080002, "", "OLE service could not bind object ."), new ErrorDef(0x80080003, "", "RPC communication failed with OLE service ."), new ErrorDef(0x80080004, "", "Bad path to object."), new ErrorDef(0x80080005, "", "Server execution failed."), new ErrorDef(0x80080006, "", "OLE service could not communicate with the object server."), new ErrorDef(0x80080007, "", "Moniker path could not be normalized ."), new ErrorDef(0x80080008, "", "Object server is stopping when OLE service contacts it."), new ErrorDef(0x80080005, "", "An invalid root block pointer was specified ."), 
                new ErrorDef(0xc0040001, "", "A subscription is not known for the specified handle."), new ErrorDef(0xc0040004, "", "The server cannot convert the data to the requested data type."), new ErrorDef(0xc0040005, "", "The requested operation cannot be done on a public group."), new ErrorDef(0xc0040006, "", "The Item's AccessRights do not allow the operation."), new ErrorDef(0xc0040007, "", "The item is no longer available in the server address space."), new ErrorDef(0xc0040008, "", "The item name does not conform to the server's syntax."), new ErrorDef(0xc0040009, "", "The filter string is not valid."), new ErrorDef(0xc004000a, "", "The item path is not known to the server."), new ErrorDef(0xc004000b, "", "The value is out of range."), new ErrorDef(0xc004000c, "", "Duplicate name not allowed."), new ErrorDef(0xc0040010, "", "The server's configuration file is an invalid format."), new ErrorDef(0xc0040011, "", "Requested Object (e.g. a public group) was not found."), new ErrorDef(0xc0040203, "", "The property name is not valid for the specified item."), new ErrorDef(0xc0048001, "", "No item list has been passed in the request."), new ErrorDef(0xc0048002, "", "Server does not support writing quality / timestamp."), new ErrorDef(0xc0048003, "", "Error in writing the quality for an item."), 
                new ErrorDef(0xc0048004, "", "The operation failed because the server is in the wrong state."), new ErrorDef(0xc0048005, "", "The value is time stamped older than the requested maximum age."), new ErrorDef(0xc0048006, "", "The item is read only and cannot be written to."), new ErrorDef(0xc0048007, "", "The item is write only and cannot be read or returned in a Write response."), new ErrorDef(0xc0048008, "", "Error in writing the timestamp for an item."), new ErrorDef(0xc0048009, "", "The operation timed out."), new ErrorDef(0xc004800a, "", "Error in writing the Vendor or Limit Bits for an item."), new ErrorDef(0xc004800b, "", "The server is currently processing another SubscriptionPolledRefresh from that client."), new ErrorDef(0xc004800c, "", "The continuation point (Browse) is not valid."), new ErrorDef(0xc004800d, "", "The time delay until the specified holdtime is too long (server specific).")
             };

            private ErrorDef GetErrDef(long num)
            {
                foreach (ErrorDef def in this.Errors)
                {
                    if (def.ErrNo == ((uint) num))
                    {
                        return def;
                    }
                }
                return null;
            }

            internal string GetErrorDefinition(long lngErr)
            {
                long num = lngErr;
                switch (num)
                {
                    case -1073479679L:
                        return "OPC_E_INVALIDHANDLE";

                    case -1073479676L:
                        return "OPC_E_BADTYPE";

                    case -1073479675L:
                        return "OPC_E_internal";

                    case -1073479674L:
                        return "OPC_E_BADRIGHTS";

                    case -1073479673L:
                        return "OPC_E_UNKNOWNITEMID";

                    case -1073479672L:
                        return "OPC_E_INVALIDITEMID";

                    case -1073479671L:
                        return "OPC_E_INVALIDFILTER";

                    case -1073479670L:
                        return "OPC_E_UNKNOWNPATH";

                    case -1073479669L:
                        return "OPC_E_RANGE";

                    case -1073479668L:
                        return "OPC_E_DUPLICATENAME";

                    case 0x4000dL:
                        return "OPC_S_UNSUPPORTEDRATE";

                    case 0x4000eL:
                        return "OPC_S_CLAMP";

                    case 0x4000fL:
                        return "OPC_S_INUSE";

                    case -1073479664L:
                        return "OPC_E_INVALIDCONFIGFILE";

                    case -1073479663L:
                        return "OPC_E_NOTFOUND";

                    case -1073479165L:
                        return "OPC_E_INVALID_PID";

                    case -1073478656L:
                        return "OPC_E_DEADBANDNOTSET";

                    case -1073478655L:
                        return "OPC_E_DEADBANDNOTSUPPORTED ";

                    case -1073478654L:
                        return "OPC_E_NOBUFFERING";

                    case -1073478653L:
                        return "OPC_E_INVALIDCONTINUATIONPOINT";

                    case 0x40404L:
                        return "OPC_S_DATAQUEUEOVERFLOW";

                    case -1073478651L:
                        return "OPC_E_RATENOTSET";

                    case -1073478650L:
                        return "OPC_E_NOTSUPPORTED";

                    case -1073475583L:
                        return "OPC_E_MAXEXCEEDED";

                    case 0x40041002L:
                        return "OPC_S_NODATA";

                    case 0x40041003L:
                        return "OPC_S_MOREDATA";

                    case -1073475580L:
                        return "OPC_E_INVALIDAGGREGATE";

                    case 0x40041005L:
                        return "OPC_S_CURRENTVALUE";

                    case 0x40041006L:
                        return "OPC_S_EXTRADATA";

                    case -2147217401L:
                        return "OPC_W_NOFILTER";

                    case -1073475576L:
                        return "OPC_E_UNKNOWNATTRID";

                    case -1073475575L:
                        return "OPC_E_NOT_AVAIL";

                    case -1073475574L:
                        return "OPC_E_INVALIDDATATYPE";

                    case -1073475573L:
                        return "OPC_E_DATAEXISTS";

                    case -1073475572L:
                        return "OPC_E_INVALIDATTRID";

                    case -1073475571L:
                        return "OPC_E_NODATAEXISTS";

                    case 0x4004100eL:
                        return "OPC_S_INSERTED";

                    case 0x4004100fL:
                        return "OPC_S_REPLACED";

                    case 0x40200L:
                        return "OPC_S_ALREADYACKED";

                    case 0x40201L:
                        return "OPC_S_INVALIDBUFFERTIME";

                    case 0x40202L:
                        return "OPC_S_INVALIDMAXSIZE";

                    case 0x40203L:
                        return "OPC_S_INVALIDKEEPALIVETIME";
                }
                switch (num)
                {
                    case -1073479165L:
                        return "OPC_E_INVALIDBRANCHNAME";

                    case -1073479164L:
                        return "OPC_E_INVALIDTIME";

                    case -1073479163L:
                        return "OPC_E_BUSY";

                    case -1073479162L:
                        return "OPC_E_NOINFO";

                    case -1073477888L:
                        return "OPCDX_E_PERSISTING";

                    case -1073477887L:
                        return "OPCDX_E_NOITEMLIST";

                    case -1073477886L:
                        return "OPCDX_E_SERVER_STATE";

                    case -1073477885L:
                        return "OPCDX_E_VERSION_MISMATCH";

                    case -1073477884L:
                        return "OPCDX_E_UNKNOWN_ITEM_PATH";

                    case -1073477883L:
                        return "OPCDX_E_UNKNOWN_ITEM_NAME";

                    case -1073477882L:
                        return "OPCDX_E_INVALID_ITEM_PATH";

                    case -1073477881L:
                        return "OPCDX_E_INVALID_ITEM_NAME";

                    case -1073477880L:
                        return "OPCDX_E_INVALID_NAME";

                    case -1073477879L:
                        return "OPCDX_E_DUPLICATE_NAME";

                    case -1073477878L:
                        return "OPCDX_E_INVALID_BROWSE_PATH";

                    case -1073477877L:
                        return "OPCDX_E_INVALID_SERVER_URL";

                    case -1073477876L:
                        return "OPCDX_E_INVALID_SERVER_TYPE";

                    case -1073477875L:
                        return "OPCDX_E_UNSUPPORTED_SERVER_TYPE";

                    case -1073477874L:
                        return "OPCDX_E_CONNECTIONS_EXIST";

                    case -1073477873L:
                        return "OPCDX_E_TOO_MANY_CONNECTIONS";

                    case -1073477872L:
                        return "OPCDX_E_OVERRIDE_BADTYPE";

                    case -1073477871L:
                        return "OPCDX_E_OVERRIDE_RANGE";

                    case -1073477870L:
                        return "OPCDX_E_SUBSTITUTE_BADTYPE";

                    case -1073477869L:
                        return "OPCDX_E_SUBSTITUTE_RANGE";

                    case -1073477868L:
                        return "OPCDX_E_INVALID_TARGET_ITEM";

                    case -1073477867L:
                        return "OPCDX_E_UNKNOWN_TARGET_ITEM";

                    case -1073477866L:
                        return "OPCDX_E_TARGET_ALREADY_CONNECTED";

                    case -1073477865L:
                        return "OPCDX_E_UNKNOWN_SERVER_NAME";

                    case -1073477864L:
                        return "OPCDX_E_UNKNOWN_SOURCE_ITEM";

                    case -1073477863L:
                        return "OPCDX_E_INVALID_SOURCE_ITEM";

                    case -1073477862L:
                        return "OPCDX_E_INVALID_QUEUE_SIZE";

                    case -1073477861L:
                        return "OPCDX_E_INVALID_DEADBAND";

                    case -1073477860L:
                        return "OPCDX_E_INVALID_CONFIG_FILE";

                    case -1073477859L:
                        return "OPCDX_E_PERSIST_FAILED";

                    case -1073477858L:
                        return "OPCDX_E_TARGET_FAULT";

                    case -1073477857L:
                        return "OPCDX_E_TARGET_NO_ACCESS";

                    case -1073477856L:
                        return "OPCDX_E_SOURCE_SERVER_FAULT";

                    case -1073477855L:
                        return "OPCDX_E_SOURCE_SERVER_NO_ACCESS";

                    case -1073477854L:
                        return "OPCDX_E_SUBSCRIPTION_FAULT";

                    case -1073477853L:
                        return "OPCDX_E_SOURCE_ITEM_BADRIGHTS";

                    case -1073477852L:
                        return "OPCDX_E_SOURCE_ITEM_BAD_QUALITY";

                    case -1073477851L:
                        return "OPCDX_E_SOURCE_ITEM_BADTYPE";

                    case -1073477850L:
                        return "OPCDX_E_SOURCE_ITEM_RANGE";

                    case -1073477849L:
                        return "OPCDX_E_SOURCE_SERVER_NOT_CONNECTED";

                    case -1073477848L:
                        return "OPCDX_E_SOURCE_SERVER_TIMEOUT";

                    case -1073477847L:
                        return "OPCDX_E_TARGET_ITEM_DISCONNECTED";

                    case -1073477846L:
                        return "OPCDX_E_TARGET_NO_WRITES_ATTEMPTED";

                    case -1073477845L:
                        return "OPCDX_E_TARGET_ITEM_BADTYPE";

                    case -1073477844L:
                        return "OPCDX_E_TARGET_ITEM_RANGE";

                    case 0x40780L:
                        return "OPCDX_S_TARGET_SUBSTITUTED";

                    case 0x40781L:
                        return "OPCDX_S_TARGET_OVERRIDEN";

                    case 0x40782L:
                        return "OPCDX_S_CLAMP";

                    case 1L:
                        return "S_FALSE";

                    case 0L:
                        return "S_OK";

                    case -2147467263L:
                        return "E_NOTIMPL";

                    case -2147024882L:
                        return "E_OUTOFMEMORY";

                    case -2147024809L:
                        return "E_INVALIDARG";

                    case -2147467262L:
                        return "E_NOINTERFACE";

                    case -2147467261L:
                        return "E_POINTER";

                    case -2147467260L:
                        return "E_ABORT";

                    case -2147467259L:
                        return "E_FAIL";

                    case -2147024890L:
                        return "E_HANDLE";

                    case -2147024891L:
                        return "E_ACCESSDENIED";

                    case -2147483647L:
                        return "E_NOTIMPL";

                    case -2147483646L:
                        return "E_OUTOFMEMORY";

                    case -2147483645L:
                        return "E_INVALIDARG";

                    case -2147483644L:
                        return "E_NOINTERFACE";

                    case -2147483643L:
                        return "E_POINTER";

                    case -2147483642L:
                        return "E_HANDLE";

                    case -2147483641L:
                        return "E_ABORT";

                    case -2147483640L:
                        return "E_FAIL";

                    case -2147483639L:
                        return "E_ACCESSDENIED";

                    case -2147483638L:
                        return "E_PENDING";
                }
                ErrorDef errDef = this.GetErrDef(num);
                if (errDef == null)
                {
                    return "Error Code not found";
                }
                return errDef.ErrSymb;
            }

            internal string GetErrorDescription(long lngErr)
            {
                string errTxt = "";
                try
                {
                    long num = lngErr;
                    switch (num)
                    {
                        case -1073479679L:
                            return "The value of the handle is invalid.";

                        case -1073479676L:
                            return "The server cannot convert the data between the specified format and/or requested data type and the canonical data type.";

                        case -1073479675L:
                            return "The requested operation cannot be done on a public group.";

                        case -1073479674L:
                            return "The item's access rights do not allow the operation.";

                        case -1073479673L:
                            return "The item ID is not defined in the server address space or no longer exists in the server address space.";

                        case -1073479672L:
                            return "The item ID does not conform to the server's syntax.";

                        case -1073479671L:
                            return "The filter string was not valid.";

                        case -1073479670L:
                            return "The item's access path is not known to the server.";

                        case -1073479669L:
                            return "The value was out of range.";

                        case -1073479668L:
                            return "Duplicate name not allowed.";

                        case 0x4000dL:
                            return "The server does not support the requested data rate but will use the closest available rate.";

                        case 0x4000eL:
                            return "A value passed to write was accepted but the output was clamped.";

                        case 0x4000fL:
                            return "The operation cannot be performed because the object is bering referenced.";

                        case -1073479664L:
                            return "The server's configuration file is an invalid format.";

                        case -1073479663L:
                            return "The requested object was not found.";

                        case -1073479165L:
                            return "The specified property ID is not valid for the item.";

                        case -1073478656L:
                            return "The item deadband has not been set for this item.";

                        case -1073478655L:
                            return "The item does not support deadband. ";

                        case -1073478654L:
                            return "The server does not support buffering of data items that are collected at a faster rate than the group update rate.";

                        case -1073478653L:
                            return "The continuation point is not valid.";

                        case 0x40404L:
                            return "Not every detected change has been returned since the server's buffer reached its limit and had to purge out the oldest data.";

                        case -1073478651L:
                            return "There is no sampling rate set for the specified item.";

                        case -1073478650L:
                            return "The server does not support writing of quality and/or timestamp.";

                        case -1073475583L:
                            return "The maximum number of values requested exceeds the server's limit.";

                        case 0x40041002L:
                            return "There is no data within the specified parameters";

                        case 0x40041003L:
                            return "There is more data satisfying the query than was returned";

                        case -1073475580L:
                            return "The aggregate requested is not valid.";

                        case 0x40041005L:
                            return "The server only returns current values for the requested item attributes.";

                        case 0x40041006L:
                            return "Additional data satisfying the query was found.";

                        case -2147217401L:
                            return "The server does not support this filter.";

                        case -1073475576L:
                            return "The server does not support this attribute.";

                        case -1073475575L:
                            return "The requested aggregate is not available for the specified item.";

                        case -1073475574L:
                            return "The supplied value for the attribute is not a correct data type.";

                        case -1073475573L:
                            return "Unable to insert - data already present.";

                        case -1073475572L:
                            return "The supplied attribute ID is not valid.";

                        case -1073475571L:
                            return "The server has no value for the specified time and item ID.";

                        case 0x4004100eL:
                            return "The requested insert occurred.";

                        case 0x4004100fL:
                            return "The requested replace occurred.";

                        case 0x40200L:
                            return "The condition has already been acknowleged";

                        case 0x40201L:
                            return "The buffer time parameter was invalid";

                        case 0x40202L:
                            return "The max size parameter was invalid";

                        case 0x40203L:
                            return "The KeepAliveTime parameter was invalid";
                    }
                    switch (num)
                    {
                        case -1073479165L:
                            return "The string was not recognized as an area name";

                        case -1073479164L:
                            return "The time does not match the latest active time";

                        case -1073479163L:
                            return "A refresh is currently in progress";

                        case -1073479162L:
                            return "Information is not available";

                        case -1073477888L:
                            return "Could not process request because the configuration is currently being persisted.";

                        case -1073477887L:
                            return "No item list was passed in the request.";

                        case -1073477886L:
                            return "The operation failed because the server is in the wrong state.";

                        case -1073477885L:
                            return "The current object version does not match the specified version.";

                        case -1073477884L:
                            return "The specified item path no longer exists in the DX server's address space.";

                        case -1073477883L:
                            return "The specified item name no longer exists in the DX server's address space.";

                        case -1073477882L:
                            return "The specified item path does not conform to the DX server syntax.";

                        case -1073477881L:
                            return "The specified item name does not conform to the DX server syntax.";

                        case -1073477880L:
                            return "The source server name or connection name does not conform to the server syntax.";

                        case -1073477879L:
                            return "The connection name or source server name is already in use.";

                        case -1073477878L:
                            return "The browse path does not conform to the DX server syntax or it cannot be modified.";

                        case -1073477877L:
                            return "The syntax of the source server URL is not correct.";

                        case -1073477876L:
                            return "The source server type is not recognized.";

                        case -1073477875L:
                            return "The source server does not support the specified server type.";

                        case -1073477874L:
                            return "The source server cannot be deleted because connections exist.";

                        case -1073477873L:
                            return "The total number of connections would exceed the maximum supported by the DX server.";

                        case -1073477872L:
                            return "The override value is not valid and overridden flag is set to true.";

                        case -1073477871L:
                            return "The override value is outside of the target item’s range and overridden flag is set to true.";

                        case -1073477870L:
                            return "The substitute  value is not valid and the enable substitute value flag is set to true.";

                        case -1073477869L:
                            return "The substitute value is outside of the target item’s range the enable substitute value flag is set to true.";

                        case -1073477868L:
                            return "The target item does not conform to the DX server syntax or the item cannot be used as a target.";

                        case -1073477867L:
                            return "The target item no longer exists in the DX server’s address space.";

                        case -1073477866L:
                            return "The target item is already connected or may not be changed in the method.";

                        case -1073477865L:
                            return "The specified source server does not exist.";

                        case -1073477864L:
                            return "The source item is no longer in the source server’s address space.";

                        case -1073477863L:
                            return "The source item does not confirm to the source server’s syntax.";

                        case -1073477862L:
                            return "The update queue size is not valid.";

                        case -1073477861L:
                            return "The deadband is not valid.";

                        case -1073477860L:
                            return "The DX server configuration file is not acessable.";

                        case -1073477859L:
                            return "Could not save the DX server configuration.";

                        case -1073477858L:
                            return "Target is online, but cannot service any request due to being in a fault state.";

                        case -1073477857L:
                            return "Target is not online and may not be accessed.";

                        case -1073477856L:
                            return "Source server is online, but cannot service any request due to being in a fault state.";

                        case -1073477855L:
                            return "Source server is not online and may not be accessed.";

                        case -1073477854L:
                            return "Source server is connected, however it could not create a subscription for the connection.";

                        case -1073477853L:
                            return "The source items access rights ddo not permit the operation.";

                        case -1073477852L:
                            return "The source item value could be used because it has bad quality.";

                        case -1073477851L:
                            return "The source item cannot be converted to the target’s data type. This error reported by the source server.";

                        case -1073477850L:
                            return "The source item is out of the range for the requested type. This error reported by the source server.";

                        case -1073477849L:
                            return "The source server is not connected.";

                        case -1073477848L:
                            return "The source server was disconnected because to failed to respond to pings.";

                        case -1073477847L:
                            return "The target item is disconnected.";

                        case -1073477846L:
                            return "The DX server has not attempted to write to the target.";

                        case -1073477845L:
                            return "The target item cannot be converted to the target’s data type.";

                        case -1073477844L:
                            return "The target item is outside the value range for the item.";

                        case 0x40780L:
                            return "The substitute value was written to the target.";

                        case 0x40781L:
                            return "The override value was written to the target.";

                        case 0x40782L:
                            return "Value written was accepted but the output was clamped.";

                        case 1L:
                            return "S_FALSE";

                        case 0L:
                            return "The call succeeded";

                        case -2147467263L:
                            return "Not implemented";

                        case -2147024882L:
                            return "Ran out of memory";

                        case -2147024809L:
                            return "One or more arguments are invalid";

                        case -2147467262L:
                            return "No such interface supported";

                        case -2147467261L:
                            return "Invalid pointer";

                        case -2147467260L:
                            return "Operation aborted";

                        case -2147467259L:
                            return "Unspecified error";

                        case -2147024890L:
                            return "Invalid handle";

                        case -2147024891L:
                            return "General access denied error";

                        case -2147483647L:
                            return "Not implemented";

                        case -2147483646L:
                            return "Ran out of memory";

                        case -2147483645L:
                            return "One or more arguments are invalid";

                        case -2147483644L:
                            return "No such interface supported";

                        case -2147483643L:
                            return "Invalid pointer";

                        case -2147483642L:
                            return "Invalid handle";

                        case -2147483641L:
                            return "Operation aborted";

                        case -2147483640L:
                            return "Unspecified error";

                        case -2147483639L:
                            return "General access denied error";

                        case -2147483638L:
                            return "The date neccessary to complete this operation is not yet available";
                    }
                    ErrorDef errDef = this.GetErrDef(num);
                    if (errDef == null)
                    {
                        return "";
                    }
                    errTxt = errDef.ErrTxt;
                }
                catch
                {
                }
                return errTxt;
            }

            internal class ErrorDef
            {
                internal uint ErrNo;
                internal string ErrSymb;
                internal string ErrTxt;

                internal ErrorDef(uint no, string symb, string txt)
                {
                    this.ErrNo = no;
                    this.ErrSymb = symb;
                    this.ErrTxt = txt;
                }
            }

            internal class Win32Errors
            {
                private static string[] Codes = new string[] { 
                    "1 Incorrect function.", "2 The system cannot find the file specified.", "3 The system cannot find the path specified.", "4 The system cannot open the file.", "5 Access is denied.", "6 The handle is invalid.", "7 The storage control blocks were destroyed.", "8 Not enough storage is available to process this command.", "9 The storage control block address is invalid.", "10 The environment is incorrect.", "11 An attempt was made to load a program with an incorrect format.", "12 The access code is invalid.", "13 The data is invalid.", "14 Not enough storage is available to complete this operation.", "15 The system cannot find the drive specified.", "16 The directory cannot be removed.", 
                    "17 The system cannot move the file to a different disk drive.", "18 There are no more files.", "19 The media is write protected.", "20 The system cannot find the device specified.", "21 The device is not ready.", "22 The device does not recognize the command.", "23 Data error (cyclic redundancy check).", "24 The program issued a command but the command length is incorrect.", "25 The drive cannot locate a specific area or track on the disk.", "26 The specified disk or diskette cannot be accessed.", "27 The drive cannot find the sector requested.", "28 The printer is out of paper.", "29 The system cannot write to the specified device.", "30 The system cannot read from the specified device.", "31 A device attached to the system is not functioning.", "32 The process cannot access the file because it is being used by another process.", 
                    "33 The process cannot access the file because another process has locked a portion of the file.", "34 The wrong diskette is in the drive.", "36 Too many files opened for sharing.", "38 Reached the end of the file.", "39 The disk is full.", "50 The network request is not supported.", "51 The remote computer is not available.", "52 A duplicate name exists on the network.", "53 The network path was not found.", "54 The network is busy.", "55 The specified network resource or device is no longer available.", "56 The network BIOS command limit has been reached.", "57 A network adapter hardware error occurred.", "58 The specified server cannot perform the requested operation.", "59 An unexpected network error occurred.", "60 The remote adapter is not compatible.", 
                    "61 The printer queue is full.", "62 Space to store the file waiting to be printed is not available on the server.", "63 Your file waiting to be printed was deleted.", "64 The specified network name is no longer available.", "65 Network access is denied.", "66 The network resource type is not correct.", "67 The network name cannot be found.", "68 The name limit for the local computer network adapter card was exceeded.", "69 The network BIOS session limit was exceeded.", "70 The remote server has been paused or is in the process of being started.", "71 No more connections can be made to this remote computer at this time because there are already as many connections as the computer can accept.", "72 The specified printer or disk device has been paused.", "80 The file exists.", "82 The directory or file cannot be created.", "83 Fail on INT 24.", "84 Storage to process this request is not available.", 
                    "85 The local device name is already in use.", "86 The specified network password is not correct.", "87 The parameter is incorrect.", "88 A write fault occurred on the network.", "89 The system cannot start another process at this time.", "100 Cannot create another system semaphore.", "101 The exclusive semaphore is owned by another process.", "102 The semaphore is set and cannot be closed.", "103 The semaphore cannot be set again.", "104 Cannot request exclusive semaphores at interrupt time.", "105 The previous ownership of this semaphore has ended.", "106 Insert the diskette for drive %1.", "107 The program stopped because an alternate diskette was not inserted.", "108 The disk is in use or locked by another process.", "109 The pipe has been ended.", "110 The system cannot open the device or file specified.", 
                    "111 The file name is too long.", "112 There is not enough space on the disk.", "113 No more internal file identifiers available.", "114 The target internal file identifier is incorrect.", "117 The IOCTL call made by the application program is not correct.", "118 The verify-on-write switch parameter value is not correct.", "119 The system does not support the command requested.", "120 This function is not supported on this system.", "121 The semaphore timeout period has expired.", "122 The data area passed to a system call is too small.", "123 The filename, directory name, or volume label syntax is incorrect.", "124 The system call level is not correct.", "125 The disk has no volume label.", "126 The specified module could not be found.", "127 The specified procedure could not be found.", "128 There are no child processes to wait for.", 
                    "129 The %1 application cannot be run in Win32 mode.", "130 Attempt to use a file handle to an open disk partition for an operation other than raw disk I/O.", "131 An attempt was made to move the file pointer before the beginning of the file.", "132 The file pointer cannot be set on the specified device or file.", "133 A JOIN or SUBST command cannot be used for a drive that contains previously joined drives.", "134 An attempt was made to use a JOIN or SUBST command on a drive that has already been joined.", "135 An attempt was made to use a JOIN or SUBST command on a drive that has already been substituted.", "136 The system tried to delete the JOIN of a drive that is not joined.", "137 The system tried to delete the substitution of a drive that is not substituted.", "138 The system tried to join a drive to a directory on a joined drive.", "139 The system tried to substitute a drive to a directory on a substituted drive.", "140 The system tried to join a drive to a directory on a substituted drive.", "141 The system tried to SUBST a drive to a directory on a joined drive.", "142 The system cannot perform a JOIN or SUBST at this time.", "143 The system cannot join or substitute a drive to or for a directory on the same drive.", "144 The directory is not a subdirectory of the root directory.", 
                    "145 The directory is not empty.", "146 The path specified is being used in a substitute.", "147 Not enough resources are available to process this command.", "148 The path specified cannot be used at this time.", "149 An attempt was made to join or substitute a drive for which a directory on the drive is the target of a previous substitute.", "150 System trace information was not specified in your CONFIG.", "151 The number of specified semaphore events for DosMuxSemWait is not correct.", "152 DosMuxSemWait did not execute; too many semaphores are already set.", "153 The DosMuxSemWait list is not correct.", "154 The volume label you entered exceeds the label character limit of the target file system.", "155 Cannot create another thread.", "156 The recipient process has refused the signal.", "157 The segment is already discarded and cannot be locked.", "158 The segment is already unlocked.", "159 The address for the thread ID is not correct.", "160 The argument string passed to DosExecPgm is not correct.", 
                    "161 The specified path is invalid.", "162 A signal is already pending.", "164 No more threads can be created in the system.", "167 Unable to lock a region of a file.", "170 The requested resource is in use.", "173 A lock request was not outstanding for the supplied cancel region.", "174 The file system does not support atomic changes to the lock type.", "180 The system detected a segment number that was not correct.", "182 The operating system cannot run %1.", "183 Cannot create a file when that file already exists.", "186 The flag passed is not correct.", "187 The specified system semaphore name was not found.", "188 The operating system cannot run %1.", "189 The operating system cannot run %1.", "190 The operating system cannot run %1.", "191 Cannot run %1 in Win32 mode.", 
                    "192 The operating system cannot run %1.", "193 Not a valid Win32 application.", "194 The operating system cannot run %1.", "195 The operating system cannot run %1.", "196 The operating system cannot run this application program.", "197 The operating system is not presently configured to run this application.", "198 The operating system cannot run %1.", "199 The operating system cannot run this application program.", "200 The code segment cannot be greater than or equal to 64K.", "201 The operating system cannot run %1.", "202 The operating system cannot run %1.", "203 The system could not find the environment option that was entered.", "205 No process in the command subtree has a signal handler.", "206 The filename or extension is too long.", "207 The ring 2 stack is in use.", "208 The global filename characters, * or ?, are entered incorrectly or too many global filename characters are specified.", 
                    "209 The signal being posted is not correct.", "210 The signal handler cannot be set.", "212 The segment is locked and cannot be reallocated.", "214 Too many dynamic-link modules are attached to this program or dynamic-link module.", "215 Cannot nest calls to LoadModule.", "216 The image file %1 is valid, but is for a machine type other than the current machine.", "230 The pipe state is invalid.", "231 All pipe instances are busy.", "232 The pipe is being closed.", "233 No process is on the other end of the pipe.", "234 More data is available.", "240 The session was canceled.", "254 The specified extended attribute name was invalid.", "255 The extended attributes are inconsistent.", "258 The wait operation timed out.", "259 No more data is available.", 
                    "266 The copy functions cannot be used.", "267 The directory name is invalid.", "275 The extended attributes did not fit in the buffer.", "276 The extended attribute file on the mounted file system is corrupt.", "277 The extended attribute table file is full.", "278 The specified extended attribute handle is invalid.", "282 The mounted file system does not support extended attributes.", "288 Attempt to release mutex not owned by caller.", "298 Too many posts were made to a semaphore.", "299 Only part of a ReadProcessMemory or WriteProcessMemory request was completed.", "300 The oplock request is denied.", "301 An invalid oplock acknowledgment was received by the system.", "302 The volume is too fragmented to complete this operation.", "317 The system cannot find message text for message number 0x%1 in the message file for %2.", "487 Attempt to access invalid address.", "534 Arithmetic result exceeded 32 bits.", 
                    "535 There is a process on other end of the pipe.", "536 Waiting for a process to open the other end of the pipe.", "994 Access to the extended attribute was denied.", "995 The I/O operation has been aborted because of either a thread exit or an application request.", "996 Overlapped I/O event is not in a signaled state.", "997 Overlapped I/O operation is in progress.", "998 Invalid access to memory location.", "999 Error performing inpage operation.", "1001 Recursion too deep; the stack overflowed.", "1002 The window cannot act on the sent message.", "1003 Cannot complete this function.", "1004 Invalid flags.", "1005 The volume does not contain a recognized file system.", "1006 The volume for a file has been externally altered so that the opened file is no longer valid.", "1007 The requested operation cannot be performed in full-screen mode.", "1008 An attempt was made to reference a token that does not exist.", 
                    "1009 The configuration registry database is corrupt.", "1010 The configuration registry key is invalid.", "1011 The configuration registry key could not be opened.", "1012 The configuration registry key could not be read.", "1013 The configuration registry key could not be written.", "1014 One of the files in the registry database had to be recovered by use of a log or alternate copy.", "1015 The registry is corrupted.", "1016 An I/O operation initiated by the registry failed unrecoverably.", "1017 The system has attempted to load or restore a file into the registry, but the specified file is not in a registry file format.", "1018 Illegal operation attempted on a registry key that has been marked for deletion.", "1019 System could not allocate the required space in a registry log.", "1020 Cannot create a symbolic link in a registry key that already has subkeys or values.", "1021 Cannot create a stable subkey under a volatile parent key.", "1022 A notify change request is being completed and the information is not being returned in the caller//s buffer.", "1051 A stop control has been sent to a service that other running services are dependent on.", "1052 The requested control is not valid for this service.", 
                    "1053 The service did not respond to the start or control request in a timely fashion.", "1054 A thread could not be created for the service.", "1055 The service database is locked.", "1056 An instance of the service is already running.", "1057 The account name is invalid or does not exist, or the password is invalid for the account name specified.", "1058 The service cannot be started, either because it is disabled or because it has no enabled devices associated with it.", "1059 Circular service dependency was specified.", "1060 The specified service does not exist as an installed service.", "1061 The service cannot accept control messages at this time.", "1062 The service has not been started.", "1063 The service process could not connect to the service controller.", "1064 An exception occurred in the service when handling the control request.", "1065 The database specified does not exist.", "1066 The service has returned a service-specific error code.", "1067 The process terminated unexpectedly.", "1068 The dependency service or group failed to start.", 
                    "1069 The service did not start due to a logon failure.", "1070 After starting, the service hung in a start-pending state.", "1071 The specified service database lock is invalid.", "1072 The specified service has been marked for deletion.", "1073 The specified service already exists.", "1074 The system is currently running with the last-known-good configuration.", "1075 The dependency service does not exist or has been marked for deletion.", "1076 The current boot has already been accepted for use as the last-known-good control set.", "1077 No attempts to start the service have been made since the last boot.", "1078 The name is already in use as either a service name or a service display name.", "1079 The account specified for this service is different from the account specified for other services running in the same process.", "1080 Failure actions can only be set for Win32 services, not for drivers.", "1081 This service runs in the same process as the service control manager.", "1082 No recovery program has been configured for this service.", "1083 The executable program that this service is configured to run in does not implement the service.", "1100 The physical end of the tape has been reached.", 
                    "1101 A tape access reached a filemark.", "1102 The beginning of the tape or a partition was encountered.", "1103 A tape access reached the end of a set of files.", "1104 No more data is on the tape.", "1105 Tape could not be partitioned.", "1106 When accessing a new tape of a multivolume partition, the current block size is incorrect.", "1107 Tape partition information could not be found when loading a tape.", "1108 Unable to lock the media eject mechanism.", "1109 Unable to unload the media.", "1110 The media in the drive may have changed.", "1111 The I/O bus was reset.", "1112 No media in drive.", "1113 No mapping for the Unicode character exists in the target multi-byte code page.", "1114 A dynamic link library (DLL) initialization routine failed.", "1115 A system shutdown is in progress.", "1116 Unable to abort the system shutdown because no shutdown was in progress.", 
                    "1117 The request could not be performed because of an I/O device error.", "1118 No serial device was successfully initialized.", "1119 Unable to open a device that was sharing an interrupt request (IRQ) with other devices.", "1120 A serial I/O operation was completed by another write to the serial port.", "1121 A serial I/O operation completed because the timeout period expired.", "1122 No ID address mark was found on the floppy disk.", "1123 Mismatch between the floppy disk sector ID field and the floppy disk controller track address.", "1124 The floppy disk controller reported an error that is not recognized by the floppy disk driver.", "1125 The floppy disk controller returned inconsistent results in its registers.", "1126 While accessing the hard disk, a recalibrate operation failed, even after retries.", "1127 While accessing the hard disk, a disk operation failed even after retries.", "1128 While accessing the hard disk, a disk controller reset was needed, but even that failed.", "1129 Physical end of tape encountered.", "1130 Not enough server storage is available to process this command.", "1131 A potential deadlock condition has been detected.", "1132 The base address or the file offset specified does not have the proper alignment.", 
                    "1140 An attempt to change the system power state was vetoed by another application or driver.", "1141 The system BIOS failed an attempt to change the system power state.", "1142 An attempt was made to create more links on a file than the file system supports.", "1150 The specified program requires a newer version of Windows.", "1151 The specified program is not a Windows or MS-DOS program.", "1152 Cannot start more than one instance of the specified program.", "1153 The specified program was written for an earlier version of Windows.", "1154 One of the library files needed to run this application is damaged.", "1155 No application is associated with the specified file for this operation.", "1156 An error occurred in sending the command to the application.", "1157 One of the library files needed to run this application cannot be found.", "1158 The current process has used all of its system allowance of handles for Window Manager objects.", "1159 The message can be used only with synchronous operations.", "1160 The indicated source element has no media.", "1161 The indicated destination element already contains media.", "1162 The indicated element does not exist.", 
                    "1163 The indicated element is part of a magazine that is not present.", "1164 The indicated device requires reinitialization due to hardware errors.", "1165 The device has indicated that cleaning is required before further operations are attempted.", "1166 The device has indicated that its door is open.", "1167 The device is not connected.", "1168 Element not found.", "1169 There was no match for the specified key in the index.", "1170 The property set specified does not exist on the object.", "1171 The point passed to GetMouseMovePointsEx is not in the buffer.", "1172 The tracking (workstation) service is not running.", "1173 The Volume ID could not be found.", "1175 Unable to remove the file to be replaced.", "1176 Unable to move the replacement file to the file to be replaced.", "1177 Unable to move the replacement file to the file to be replaced.", "1178 The volume change journal is being deleted.", "1179 The volume change journal service is not active.", 
                    "1180 A file was found, but it may not be the correct file.", "1181 The journal entry has been deleted from the journal.", "1200 The specified device name is invalid.", "1201 The device is not currently connected but it is a remembered connection.", "1202 The local device name has a remembered connection to another network resource.", "1203 No network provider accepted the given network path.", "1204 The specified network provider name is invalid.", "1205 Unable to open the network connection profile.", "1206 The network connection profile is corrupted.", "1207 Cannot enumerate a noncontainer.", "1208 An extended error has occurred.", "1209 The format of the specified group name is invalid.", "1210 The format of the specified computer name is invalid.", "1211 The format of the specified event name is invalid.", "1212 The format of the specified domain name is invalid.", "1213 The format of the specified service name is invalid.", 
                    "1214 The format of the specified network name is invalid.", "1215 The format of the specified share name is invalid.", "1216 The format of the specified password is invalid.", "1217 The format of the specified message name is invalid.", "1218 The format of the specified message destination is invalid.", "1219 The credentials supplied conflict with an existing set of credentials.", "1220 An attempt was made to establish a session to a network server, but there are already too many sessions established to that server.", "1221 The workgroup or domain name is already in use by another computer on the network.", "1222 The network is not present or not started.", "1223 The operation was canceled by the user.", "1224 The requested operation cannot be performed on a file with a user-mapped section open.", "1225 The remote system refused the network connection.", "1226 The network connection was gracefully closed.", "1227 The network transport endpoint already has an address associated with it.", "1228 An address has not yet been associated with the network endpoint.", "1229 An operation was attempted on a nonexistent network connection.", 
                    "1230 An invalid operation was attempted on an active network connection.", "1231 The network location cannot be reached.", "1232 The network location cannot be reached.", "1233 The network location cannot be reached.", "1234 No service is operating at the destination network endpoint on the remote system.", "1235 The request was aborted.", "1236 The network connection was aborted by the local system.", "1237 The operation could not be completed.", "1238 A connection to the server could not be made because the limit on the number of concurrent connections for this account has been reached.", "1239 Attempting to log in during an unauthorized time of day for this account.", "1240 The account is not authorized to log in from this station.", "1241 The network address could not be used for the operation requested.", "1242 The service is already registered.", "1243 The specified service does not exist.", "1244 The operation being requested was not performed because the user has not been authenticated.", "1245 The operation being requested was not performed because the user has not logged on to the network.", 
                    "1246 Continue with work in progress.", "1247 An attempt was made to perform an initialization operation when initialization has already been completed.", "1248 No more local devices.", "1249 The specified site does not exist.", "1250 A domain controller with the specified name already exists.", "1251 This operation is supported only when you are connected to the server.", "1252 The group policy framework should call the extension even if there are no changes.", "1253 The specified user does not have a valid profile.", "1254 This operation is not supported on a Microsoft Small Business Server.", "1300 Not all privileges referenced are assigned to the caller.", "1301 Some mapping between account names and security IDs was not done.", "1302 No system quota limits are specifically set for this account.", "1303 No encryption key is available.", "1304 The password is too complex to be converted to a LAN Manager password.", "1305 The revision level is unknown.", "1306 Indicates two revision levels are incompatible.", 
                    "1307 This security ID may not be assigned as the owner of this object.", "1308 This security ID may not be assigned as the primary group of an object.", "1309 An attempt has been made to operate on an impersonation token by a thread that is not currently impersonating a client.", "1310 The group may not be disabled.", "1311 There are currently no logon servers available to service the logon request.", "1312 A specified logon session does not exist.", "1313 A specified privilege does not exist.", "1314 A required privilege is not held by the client.", "1315 The name provided is not a properly formed account name.", "1316 The specified user already exists.", "1317 The specified user does not exist.", "1318 The specified group already exists.", "1319 The specified group does not exist.", "1320 Either the specified user account is already a member of the specified group, or the specified group cannot be deleted because it contains a member.", "1321 The specified user account is not a member of the specified group account.", "1322 The last remaining administration account cannot be disabled or deleted.", 
                    "1323 Unable to update the password.", "1324 Unable to update the password.", "1325 Unable to update the password.", "1326 Logon failure: unknown user name or bad password.", "1327 Logon failure: user account restriction.", "1328 Logon failure: account logon time restriction violation.", "1329 Logon failure: user not allowed to log on to this computer.", "1330 Logon failure: the specified account password has expired.", "1331 Logon failure: account currently disabled.", "1332 No mapping between account names and security IDs was done.", "1333 Too many local user identifiers (LUIDs) were requested at one time.", "1334 No more local user identifiers (LUIDs) are available.", "1335 The subauthority part of a security ID is invalid for this particular use.", "1336 The access control list (ACL) structure is invalid.", "1337 The security ID structure is invalid.", "1338 The security descriptor structure is invalid.", 
                    "1340 The inherited access control list (ACL) or access control entry (ACE) could not be built.", "1341 The server is currently disabled.", "1342 The server is currently enabled.", "1343 The value provided was an invalid value for an identifier authority.", "1344 No more memory is available for security information updates.", "1345 The specified attributes are invalid, or incompatible with the attributes for the group as a whole.", "1346 Either a required impersonation level was not provided, or the provided impersonation level is invalid.", "1347 Cannot open an anonymous level security token.", "1348 The validation information class requested was invalid.", "1349 The type of the token is inappropriate for its attempted use.", "1350 Unable to perform a security operation on an object that has no associated security.", "1351 Configuration information could not be read from the domain controller, either because the machine is unavailable, or access has been denied.", "1352 The security account manager (SAM) or local security authority (LSA) server was in the wrong state to perform the security operation.", "1353 The domain was in the wrong state to perform the security operation.", "1354 This operation is only allowed for the Primary Domain Controller of the domain.", "1355 The specified domain either does not exist or could not be contacted.", 
                    "1356 The specified domain already exists.", "1357 An attempt was made to exceed the limit on the number of domains per server.", "1358 Unable to complete the requested operation because of either a catastrophic media failure or a data structure corruption on the disk.", "1359 An internal error occurred.", "1360 Generic access types were contained in an access mask which should already be mapped to nongeneric types.", "1361 A security descriptor is not in the right format (absolute or self-relative).", "1362 The requested action is restricted for use by logon processes only.", "1363 Cannot start a new logon session with an ID that is already in use.", "1364 A specified authentication package is unknown.", "1365 The logon session is not in a state that is consistent with the requested operation.", "1366 The logon session ID is already in use.", "1367 A logon request contained an invalid logon type value.", "1368 Unable to impersonate using a named pipe until data has been read from that pipe.", "1369 The transaction state of a registry subtree is incompatible with the requested operation.", "1370 An internal security database corruption has been encountered.", "1371 Cannot perform this operation on built-in accounts.", 
                    "1372 Cannot perform this operation on this built-in special group.", "1373 Cannot perform this operation on this built-in special user.", "1374 The user cannot be removed from a group because the group is currently the user//s primary group.", "1375 The token is already in use as a primary token.", "1376 The specified local group does not exist.", "1377 The specified account name is not a member of the local group.", "1378 The specified account name is already a member of the local group.", "1379 The specified local group already exists.", "1380 Logon failure: the user has not been granted the requested logon type at this computer.", "1381 The maximum number of secrets that may be stored in a single system has been exceeded.", "1382 The length of a secret exceeds the maximum length allowed.", "1383 The local security authority database contains an internal inconsistency.", "1384 During a logon attempt, the user//s security context accumulated too many security IDs.", "1385 Logon failure: the user has not been granted the requested logon type at this computer.", "1386 A cross-encrypted password is necessary to change a user password.", "1387 A new member could not be added to or removed from the local group because the member does not exist.", 
                    "1388 A new member could not be added to a local group because the member has the wrong account type.", "1389 Too many security IDs have been specified.", "1390 A cross-encrypted password is necessary to change this user password.", "1391 Indicates an ACL contains no inheritable components.", "1392 The file or directory is corrupted and unreadable.", "1393 The disk structure is corrupted and unreadable.", "1394 There is no user session key for the specified logon session.", "1395 The service being accessed is licensed for a particular number of connections.", "1396 Logon Failure: The target account name is incorrect.", "1397 Mutual Authentication failed.", "1398 There is a time difference between the client and server.", "1399 This operation can not be performed on the current domain.", "1400 Invalid window handle.", "1401 Invalid menu handle.", "1402 Invalid cursor handle.", "1403 Invalid accelerator table handle.", 
                    "1404 Invalid hook handle.", "1405 Invalid handle to a multiple-window position structure.", "1406 Cannot create a top-level child window.", "1407 Cannot find window class.", "1408 Invalid window; it belongs to other thread.", "1409 Hot key is already registered.", "1410 Class already exists.", "1411 Class does not exist.", "1412 Class still has open windows.", "1413 Invalid index.", "1414 Invalid icon handle.", "1415 Using private DIALOG window words.", "1416 The list box identifier was not found.", "1417 No wildcards were found.", "1418 Thread does not have a clipboard open.", "1419 Hot key is not registered.", 
                    "1420 The window is not a valid dialog window.", "1421 Control ID not found.", "1422 Invalid message for a combo box because it does not have an edit control.", "1423 The window is not a combo box.", "1424 Height must be less than 256.", "1425 Invalid device context (DC) handle.", "1426 Invalid hook procedure type.", "1427 Invalid hook procedure.", "1428 Cannot set nonlocal hook without a module handle.", "1429 This hook procedure can only be set globally.", "1430 The journal hook procedure is already installed.", "1431 The hook procedure is not installed.", "1432 Invalid message for single-selection list box.", "1433 LBSETCOUNT sent to non-lazy list box.", "1434 This list box does not support tab stops.", "1435 Cannot destroy object created by another thread.", 
                    "1436 Child windows cannot have menus.", "1437 The window does not have a system menu.", "1438 Invalid message box style.", "1439 Invalid system-wide (SPI*) parameter.", "1440 Screen already locked.", "1441 All handles to windows in a multiple-window position structure must have the same parent.", "1442 The window is not a child window.", "1443 Invalid GW* command.", "1444 Invalid thread identifier.", "1445 Cannot process a message from a window that is not a multiple document interface (MDI) window.", "1446 Popup menu already active.", "1447 The window does not have scroll bars.", "1448 Scroll bar range cannot be greater than MAXLONG.", "1449 Cannot show or remove the window in the way specified.", "1450 Insufficient system resources exist to complete the requested service.", "1451 Insufficient system resources exist to complete the requested service.", 
                    "1452 Insufficient system resources exist to complete the requested service.", "1453 Insufficient quota to complete the requested service.", "1454 Insufficient quota to complete the requested service.", "1455 The paging file is too small for this operation to complete.", "1456 A menu item was not found.", "1457 Invalid keyboard layout handle.", "1458 Hook type not allowed.", "1459 This operation requires an interactive window station.", "1460 This operation returned because the timeout period expired.", "1461 Invalid monitor handle.", "1500 The event log file is corrupted.", "1501 No event log file could be opened, so the event logging service did not start.", "1502 The event log file is full.", "1503 The event log file has changed between read operations.", "1601 The Windows Installer service could not be accessed.", "1602 User cancelled installation.", 
                    "1603 Fatal error during installation.", "1604 Installation suspended, incomplete.", "1605 This action is only valid for products that are currently installed.", "1606 Feature ID not registered.", "1607 Component ID not registered.", "1608 Unknown property.", "1609 Handle is in an invalid state.", "1610 The configuration data for this product is corrupt.", "1611 Component qualifier not present.", "1612 The installation source for this product is not available.", "1613 This installation package cannot be installed by the Windows Installer service.", "1614 Product is uninstalled.", "1615 SQL query syntax invalid or unsupported.", "1616 Record field does not exist.", "1617 The device has been removed.", "1618 Another installation is already in progress.", 
                    "1619 This installation package could not be opened.", "1620 This installation package could not be opened.", "1621 There was an error starting the Windows Installer service user interface.", "1622 Error opening installation log file.", "1623 The language of this installation package is not supported by your system.", "1624 Error applying transforms.", "1625 This installation is forbidden by system policy.", "1626 Function could not be executed.", "1627 Function failed during execution.", "1628 Invalid or unknown table specified.", "1629 Data supplied is of wrong type.", "1630 Data of this type is not supported.", "1631 The Windows Installer service failed to start.", "1632 The temp folder is either full or inaccessible.", "1633 This installation package is not supported by this processor type.", "1634 Component not used on this computer.", 
                    "1635 This patch package could not be opened.", "1636 This patch package could not be opened.", "1637 This patch package cannot be processed by the Windows Installer service.", "1638 Another version of this product is already installed.", "1639 Invalid command line argument.", "1640 Only administrators have permission to add, remove, or configure server software during a Terminal Services remote session.", "1641 The requested operation completed successfully.", "1642 The upgrade patch cannot be installed by the Windows Installer service because the program to be upgraded may be missing, or the upgrade patch may update a different version of the program.", "1700 The string binding is invalid.", "1701 The binding handle is not the correct type.", "1702 The binding handle is invalid.", "1703 The RPC protocol sequence is not supported.", "1704 The RPC protocol sequence is invalid.", "1705 The string universal unique identifier (UUID) is invalid.", "1706 The endpoint format is invalid.", "1707 The network address is invalid.", 
                    "1708 No endpoint was found.", "1709 The timeout value is invalid.", "1710 The object universal unique identifier (UUID) was not found.", "1711 The object universal unique identifier (UUID) has already been registered.", "1712 The type universal unique identifier (UUID) has already been registered.", "1713 The RPC server is already listening.", "1714 No protocol sequences have been registered.", "1715 The RPC server is not listening.", "1716 The manager type is unknown.", "1717 The interface is unknown.", "1718 There are no bindings.", "1719 There are no protocol sequences.", "1720 The endpoint cannot be created.", "1721 Not enough resources are available to complete this operation.", "1722 The RPC server is unavailable.", "1723 The RPC server is too busy to complete this operation.", 
                    "1724 The network options are invalid.", "1725 There are no remote procedure calls active on this thread.", "1726 The remote procedure call failed.", "1727 The remote procedure call failed and did not execute.", "1728 A remote procedure call (RPC) protocol error occurred.", "1730 The transfer syntax is not supported by the RPC server.", "1732 The universal unique identifier (UUID) type is not supported.", "1733 The tag is invalid.", "1734 The array bounds are invalid.", "1735 The binding does not contain an entry name.", "1736 The name syntax is invalid.", "1737 The name syntax is not supported.", "1739 No network address is available to use to construct a universal unique identifier (UUID).", "1740 The endpoint is a duplicate.", "1741 The authentication type is unknown.", "1742 The maximum number of calls is too small.", 
                    "1743 The string is too long.", "1744 The RPC protocol sequence was not found.", "1745 The procedure number is out of range.", "1746 The binding does not contain any authentication information.", "1747 The authentication service is unknown.", "1748 The authentication level is unknown.", "1749 The security context is invalid.", "1750 The authorization service is unknown.", "1751 The entry is invalid.", "1752 The server endpoint cannot perform the operation.", "1753 There are no more endpoints available from the endpoint mapper.", "1754 No interfaces have been exported.", "1755 The entry name is incomplete.", "1756 The version option is invalid.", "1757 There are no more members.", "1758 There is nothing to unexport.", 
                    "1759 The interface was not found.", "1760 The entry already exists.", "1761 The entry is not found.", "1762 The name service is unavailable.", "1763 The network address family is invalid.", "1764 The requested operation is not supported.", "1765 No security context is available to allow impersonation.", "1766 An internal error occurred in a remote procedure call (RPC).", "1767 The RPC server attempted an integer division by zero.", "1768 An addressing error occurred in the RPC server.", "1769 A floating-point operation at the RPC server caused a division by zero.", "1770 A floating-point underflow occurred at the RPC server.", "1771 A floating-point overflow occurred at the RPC server.", "1772 The list of RPC servers available for the binding of auto handles has been exhausted.", "1773 Unable to open the character translation table file.", "1774 The file containing the character translation table has fewer than 512 bytes.", 
                    "1775 A null context handle was passed from the client to the host during a remote procedure call.", "1777 The context handle changed during a remote procedure call.", "1778 The binding handles passed to a remote procedure call do not match.", "1779 The stub is unable to get the remote procedure call handle.", "1780 A null reference pointer was passed to the stub.", "1781 The enumeration value is out of range.", "1782 The byte count is too small.", "1783 The stub received bad data.", "1784 The supplied user buffer is not valid for the requested operation.", "1785 The disk media is not recognized.", "1786 The workstation does not have a trust secret.", "1787 The security database on the server does not have a computer account for this workstation trust relationship.", "1788 The trust relationship between the primary domain and the trusted domain failed.", "1789 The trust relationship between this workstation and the primary domain failed.", "1790 The network logon failed.", "1791 A remote procedure call is already in progress for this thread.", 
                    "1792 An attempt was made to logon, but the network logon service was not started.", "1793 The user//s account has expired.", "1794 The redirector is in use and cannot be unloaded.", "1795 The specified printer driver is already installed.", "1796 The specified port is unknown.", "1797 The printer driver is unknown.", "1798 The print processor is unknown.", "1799 The specified separator file is invalid.", "1800 The specified priority is invalid.", "1801 The printer name is invalid.", "1802 The printer already exists.", "1803 The printer command is invalid.", "1804 The specified datatype is invalid.", "1805 The environment specified is invalid.", "1806 There are no more bindings.", "1807 The account used is an interdomain trust account.", 
                    "1808 The account used is a computer account.", "1809 The account used is a server trust account.", "1810 The name or security ID (SID) of the domain specified is inconsistent with the trust information for that domain.", "1811 The server is in use and cannot be unloaded.", "1812 The specified image file did not contain a resource section.", "1813 The specified resource type cannot be found in the image file.", "1814 The specified resource name cannot be found in the image file.", "1815 The specified resource language ID cannot be found in the image file.", "1816 Not enough quota is available to process this command.", "1817 No interfaces have been registered.", "1818 The remote procedure call was cancelled.", "1819 The binding handle does not contain all required information.", "1820 A communications failure occurred during a remote procedure call.", "1821 The requested authentication level is not supported.", "1822 No principal name registered.", "1823 The error specified is not a valid Windows RPC error code.", 
                    "1824 A UUID that is valid only on this computer has been allocated.", "1825 A security package specific error occurred.", "1826 Thread is not canceled.", "1827 Invalid operation on the encoding/decoding handle.", "1828 Incompatible version of the serializing package.", "1829 Incompatible version of the RPC stub.", "1830 The RPC pipe object is invalid or corrupted.", "1831 An invalid operation was attempted on an RPC pipe object.", "1832 Unsupported RPC pipe version.", "1898 The group member was not found.", "1899 The endpoint mapper database entry could not be created.", "1900 The object universal unique identifier (UUID) is the nil UUID.", "1901 The specified time is invalid.", "1902 The specified form name is invalid.", "1903 The specified form size is invalid.", "1904 The specified printer handle is already being waited on.", 
                    "1905 The specified printer has been deleted.", "1906 The state of the printer is invalid.", "1907 The user//s password must be changed before logging on the first time.", "1908 Could not find the domain controller for this domain.", "1909 The referenced account is currently locked out and may not be logged on to.", "1910 The object exporter specified was not found.", "1911 The object specified was not found.", "1912 The object resolver set specified was not found.", "1913 Some data remains to be sent in the request buffer.", "1914 Invalid asynchronous remote procedure call handle.", "1915 Invalid asynchronous RPC call handle for this operation.", "1916 The RPC pipe object has already been closed.", "1917 The RPC call completed before all pipes were processed.", "1918 No more data is available from the RPC pipe.", "1919 No site name is available for this machine.", "1920 The file can not be accessed by the system.", 
                    "1921 The name of the file cannot be resolved by the system.", "1922 The entry is not of the expected type.", "1923 Not all object UUIDs could be exported to the specified entry.", "1924 Interface could not be exported to the specified entry.", "1925 The specified profile entry could not be added.", "1926 The specified profile element could not be added.", "1927 The specified profile element could not be removed.", "1928 The group element could not be added.", "1929 The group element could not be removed.", "2000 The pixel format is invalid.", "2001 The specified driver is invalid.", "2002 The window style or class attribute is invalid for this operation.", "2003 The requested metafile operation is not supported.", "2004 The requested transformation operation is not supported.", "2005 The requested clipping operation is not supported.", "2010 The specified color management module is invalid.", 
                    "2011 The specified color profile is invalid.", "2012 The specified tag was not found.", "2013 A required tag is not present.", "2014 The specified tag is already present.", "2015 The specified color profile is not associated with any device.", "2016 The specified color profile was not found.", "2017 The specified color space is invalid.", "2018 Image Color Management is not enabled.", "2019 There was an error while deleting the color transform.", "2020 The specified color transform is invalid.", "2021 The specified transform does not match the bitmap//s color space.", "2022 The specified named color index is not present in the profile.", "2108 The network connection was made successfully, but the user had to be prompted for a password other than the one originally specified.", "2202 The specified username is invalid.", "2250 This network connection does not exist.", "2401 This network connection has files open or requests pending.", 
                    "2402 Active connections still exist.", "2404 The device is in use by an active process and cannot be disconnected.", "3000 The specified print monitor is unknown.", "3001 The specified printer driver is currently in use.", "3002 The spool file was not found.", "3003 A StartDocPrinter call was not issued.", "3004 An AddJob call was not issued.", "3005 The specified print processor has already been installed.", "3006 The specified print monitor has already been installed.", "3007 The specified print monitor does not have the required functions.", "3008 The specified print monitor is currently in use.", "3009 The requested operation is not allowed when there are jobs queued to the printer.", "3010 The requested operation is successful.", "3011 The requested operation is successful.", "3012 No printers were found.", "4000 WINS encountered an error while processing the command.", 
                    "4001 The local WINS can not be deleted.", "4002 The importation from the file failed.", "4003 The backup failed.", "4004 The backup failed.", "4005 The name does not exist in the WINS database.", "4006 Replication with a nonconfigured partner is not allowed.", "4100 The DHCP client has obtained an IP address that is already in use on the network.", "4200 The GUID passed was not recognized as valid by a WMI data provider.", "4201 The instance name passed was not recognized as valid by a WMI data provider.", "4202 The data item ID passed was not recognized as valid by a WMI data provider.", "4203 The WMI request could not be completed and should be retried.", "4204 The WMI data provider could not be located.", "4205 The WMI data provider references an instance set that has not been registered.", "4206 The WMI data block or event notification has already been enabled.", "4207 The WMI data block is no longer available.", "4208 The WMI data service is not available.", 
                    "4209 The WMI data provider failed to carry out the request.", "4210 The WMI MOF information is not valid.", "4211 The WMI registration information is not valid.", "4212 The WMI data block or event notification has already been disabled.", "4213 The WMI data item or data block is read only.", "4214 The WMI data item or data block could not be changed.", "4300 The media identifier does not represent a valid medium.", "4301 The library identifier does not represent a valid library.", "4302 The media pool identifier does not represent a valid media pool.", "4303 The drive and medium are not compatible or exist in different libraries.", "4304 The medium currently exists in an offline library and must be online to perform this operation.", "4305 The operation cannot be performed on an offline library.", "4306 The library, drive, or media pool is empty.", "4307 The library, drive, or media pool must be empty to perform this operation.", "4308 No media is currently available in this media pool or library.", "4309 A resource required for this operation is disabled.", 
                    "4310 The media identifier does not represent a valid cleaner.", "4311 The drive cannot be cleaned or does not support cleaning.", "4312 The object identifier does not represent a valid object.", "4313 Unable to read from or write to the database.", "4314 The database is full.", "4315 The medium is not compatible with the device or media pool.", "4316 The resource required for this operation does not exist.", "4317 The operation identifier is not valid.", "4318 The media is not mounted or ready for use.", "4319 The device is not ready for use.", "4320 The operator or administrator has refused the request.", "4321 The drive identifier does not represent a valid drive.", "4322 Library is full.", "4323 The transport cannot access the medium.", "4324 Unable to load the medium into the drive.", "4325 Unable to retrieve status about the drive.", 
                    "4326 Unable to retrieve status about the slot.", "4327 Unable to retrieve status about the transport.", "4328 Cannot use the transport because it is already in use.", "4329 Unable to open or close the inject/eject port.", "4330 Unable to eject the media because it is in a drive.", "4331 A cleaner slot is already reserved.", "4332 A cleaner slot is not reserved.", "4333 The cleaner cartridge has performed the maximum number of drive cleanings.", "4334 Unexpected on-medium identifier.", "4335 The last remaining item in this group or resource cannot be deleted.", "4336 The message provided exceeds the maximum size allowed for this parameter.", "4337 The volume contains system or paging files.", "4338 The media type cannot be removed from this library since at least one drive in the library reports it can support this media type.", "4339 This offline media cannot be mounted on this system since no enabled drives are present which can be used.", "4350 The remote storage service was not able to recall the file.", "4351 The remote storage service is not operational at this time.", 
                    "4352 The remote storage service encountered a media error.", "4390 The file or directory is not a reparse point.", "4391 The reparse point attribute cannot be set because it conflicts with an existing attribute.", "4392 The data present in the reparse point buffer is invalid.", "4393 The tag present in the reparse point buffer is invalid.", "4394 There is a mismatch between the tag specified in the request and the tag present in the reparse point.", "4500 Single Instance Storage is not available on this volume.", "5001 The cluster resource cannot be moved to another group because other resources are dependent on it.", "5002 The cluster resource dependency cannot be found.", "5003 The cluster resource cannot be made dependent on the specified resource because it is already dependent.", "5004 The cluster resource is not online.", "5005 A cluster node is not available for this operation.", "5006 The cluster resource is not available.", "5007 The cluster resource could not be found.", "5008 The cluster is being shut down.", "5009 A cluster node cannot be evicted from the cluster unless the node is down.", 
                    "5010 The object already exists.", "5011 The object is already in the list.", "5012 The cluster group is not available for any new requests.", "5013 The cluster group could not be found.", "5014 The operation could not be completed because the cluster group is not online.", "5015 The cluster node is not the owner of the resource.", "5016 The cluster node is not the owner of the group.", "5017 The cluster resource could not be created in the specified resource monitor.", "5018 The cluster resource could not be brought online by the resource monitor.", "5019 The operation could not be completed because the cluster resource is online.", "5020 The cluster resource could not be deleted or brought offline because it is the quorum resource.", "5021 The cluster could not make the specified resource a quorum resource because it is not capable of being a quorum resource.", "5022 The cluster software is shutting down.", "5023 The group or resource is not in the correct state to perform the requested operation.", "5024 The properties were stored but not all changes will take effect until the next time the resource is brought online.", "5025 The cluster could not make the specified resource a quorum resource because it does not belong to a shared storage class.", 
                    "5026 The cluster resource could not be deleted since it is a core resource.", "5027 The quorum resource failed to come online.", "5028 The quorum log could not be created or mounted successfully.", "5029 The cluster log is corrupt.", "5030 The record could not be written to the cluster log since it exceeds the maximum size.", "5031 The cluster log exceeds its maximum size.", "5032 No checkpoint record was found in the cluster log.", "5033 The minimum required disk space needed for logging is not available.", "5034 The cluster node failed to take control of the quorum resource because the resource is owned by another active node.", "5035 A cluster network is not available for this operation.", "5036 A cluster node is not available for this operation.", "5037 All cluster nodes must be running to perform this operation.", "5038 A cluster resource failed.", "5039 The cluster node is not valid.", "5040 The cluster node already exists.", "5041 A node is in the process of joining the cluster.", 
                    "5042 The cluster node was not found.", "5043 The cluster local node information was not found.", "5044 The cluster network already exists.", "5045 The cluster network was not found.", "5046 The cluster network interface already exists.", "5047 The cluster network interface was not found.", "5048 The cluster request is not valid for this object.", "5049 The cluster network provider is not valid.", "5050 The cluster node is down.", "5051 The cluster node is not reachable.", "5052 The cluster node is not a member of the cluster.", "5053 A cluster join operation is not in progress.", "5054 The cluster network is not valid.", "5056 The cluster node is up.", "5057 The cluster IP address is already in use.", "5058 The cluster node is not paused.", 
                    "5059 No cluster security context is available.", "5060 The cluster network is not configured for internal cluster communication.", "5061 The cluster node is already up.", "5062 The cluster node is already down.", "5063 The cluster network is already online.", "5064 The cluster network is already offline.", "5065 The cluster node is already a member of the cluster.", "5066 The cluster network is the only one configured for internal cluster communication between two or more active cluster nodes.", "5067 One or more cluster resources depend on the network to provide service to clients.", "5068 This operation cannot be performed on the cluster resource as it the quorum resource.", "5069 The cluster quorum resource is not allowed to have any dependencies.", "5070 The cluster node is paused.", "5071 The cluster resource cannot be brought online.", "5072 The cluster node is not ready to perform the requested operation.", "5073 The cluster node is shutting down.", "5074 The cluster join operation was aborted.", 
                    "5075 The cluster join operation failed due to incompatible software versions between the joining node and its sponsor.", "5076 This resource cannot be created because the cluster has reached the limit on the number of resources it can monitor.", "5077 The system configuration changed during the cluster join or form operation.", "5078 The specified resource type was not found.", "5079 The specified node does not support a resource of this type.", "5080 The specified resource name is supported by this resource DLL.", "5081 No authentication package could be registered with the RPC server.", "5082 You cannot bring the group online because the owner of the group is not in the preferred list for the group.", "5083 The join operation failed because the cluster database sequence number has changed or is incompatible with the locker node.", "5084 The resource monitor will not allow the fail operation to be performed while the resource is in its current state.", "5085 A non locker code got a request to reserve the lock for making global updates.", "5086 The quorum disk could not be located by the cluster service.", "5087 The backup up cluster database is possibly corrupt.", "5088 A DFS root already exists in this cluster node.", "5089 An attempt to modify a resource property failed because it conflicts with another existing property.", "6000 The specified file could not be encrypted.", 
                    "6001 The specified file could not be decrypted.", "6002 The specified file is encrypted and the user does not have the ability to decrypt it.", "6003 There is no valid encryption recovery policy configured for this system.", "6004 The required encryption driver is not loaded for this system.", "6005 The file was encrypted with a different encryption driver than is currently loaded.", "6006 There are no EFS keys defined for the user.", "6007 The specified file is not encrypted.", "6008 The specified file is not in the defined EFS export format.", "6009 The specified file is read only.", "6010 The directory has been disabled for encryption.", "6011 The server is not trusted for remote encryption operation.", "6012 Recovery policy configured for this system contains invalid recovery certificate.", "6118 The list of servers for this workgroup is not currently available.", "6200 The Task Scheduler service must be configured to run in the System account to function properly.", "7001 The specified session name is invalid.", "7002 The specified protocol driver is invalid.", 
                    "7003 The specified protocol driver was not found in the system path.", "7004 The specified terminal connection driver was not found in the system path.", "7005 A registry key for event logging could not be created for this session.", "7006 A service with the same name already exists on the system.", "7007 A close operation is pending on the session.", "7008 There are no free output buffers available.", "7009 The MODEM.", "7010 The modem name was not found in MODEM.", "7011 The modem did not accept the command sent to it.", "7012 The modem did not respond to the command sent to it.", "7013 Carrier detect has failed or carrier has been dropped due to disconnect.", "7014 Dial tone not detected within the required time.", "7015 Busy signal detected at remote site on callback.", "7016 Voice detected at remote site on callback.", "7017 Transport driver error.", "7022 The specified session cannot be found.", 
                    "7023 The specified session name is already in use.", "7024 The requested operation cannot be completed because the terminal connection is currently busy processing a connect, disconnect, reset, or delete operation.", "7025 An attempt has been made to connect to a session whose video mode is not supported by the current client.", "7035 The application attempted to enable DOS graphics mode.", "7037 Your interactive logon privilege has been disabled.", "7038 The requested operation can be performed only on the system console.", "7040 The client failed to respond to the server connect message.", "7041 Disconnecting the console session is not supported.", "7042 Reconnecting a disconnected session to the console is not supported.", "7044 The request to control another session remotely was denied.", "7045 The requested session access is denied.", "7049 The specified terminal connection driver is invalid.", "7050 The requested session cannot be controlled remotely.", "7051 The requested session is not configured to allow remote control.", "7052 Your request to connect to this Terminal Server has been rejected.", "7053 Your request to connect to this Terminal Server has been rejected.", 
                    "7054 The system has reached its licensed logon limit.", "7055 The client you are using is not licensed to use this system.", "7056 The system license has expired.", "8001 The file replication service API was called incorrectly.", "8002 The file replication service cannot be started.", "8003 The file replication service cannot be stopped.", "8004 The file replication service API terminated the request.", "8005 The file replication service terminated the request.", "8006 The file replication service cannot be contacted.", "8007 The file replication service cannot satisfy the request because the user has insufficient privileges.", "8008 The file replication service cannot satisfy the request because authenticated RPC is not available.", "8009 The file replication service cannot satisfy the request because the user has insufficient privileges on the domain controller.", "8010 The file replication service cannot satisfy the request because authenticated RPC is not available on the domain controller.", "8011 The file replication service cannot communicate with the file replication service on the domain controller.", "8012 The file replication service on the domain controller cannot communicate with the file replication service on this computer.", "8013 The file replication service cannot populate the system volume because of an internal error.", 
                    "8014 The file replication service cannot populate the system volume because of an internal timeout.", "8015 The file replication service cannot process the request.", "8016 The file replication service cannot stop replicating the system volume because of an internal error.", "8017 The file replication service detected an invalid parameter.", "8200 An error occurred while installing the directory service.", "8201 The directory service evaluated group memberships locally.", "8202 The specified directory service attribute or value does not exist.", "8203 The attribute syntax specified to the directory service is invalid.", "8204 The attribute type specified to the directory service is not defined.", "8205 The specified directory service attribute or value already exists.", "8206 The directory service is busy.", "8207 The directory service is unavailable.", "8208 The directory service was unable to allocate a relative identifier.", "8209 The directory service has exhausted the pool of relative identifiers.", "8210 The requested operation could not be performed because the directory service is not the master for that type of operation.", "8211 The directory service was unable to initialize the subsystem that allocates relative identifiers.", 
                    "8212 The requested operation did not satisfy one or more constraints associated with the class of the object.", "8213 The directory service can perform the requested operation only on a leaf object.", "8214 The directory service cannot perform the requested operation on the RDN attribute of an object.", "8215 The directory service detected an attempt to modify the object class of an object.", "8216 The requested cross-domain move operation could not be performed.", "8217 Unable to contact the global catalog server.", "8218 The policy object is shared and can only be modified at the root.", "8219 The policy object does not exist.", "8220 The requested policy information is only in the directory service.", "8221 A domain controller promotion is currently active.", "8222 A domain controller promotion is not currently active.", "8224 An operations error occurred.", "8225 A protocol error occurred.", "8226 The time limit for this request was exceeded.", "8227 The size limit for this request was exceeded.", "8228 The administrative limit for this request was exceeded.", 
                    "8229 The compare response was false.", "8230 The compare response was true.", "8231 The requested authentication method is not supported by the server.", "8232 A more secure authentication method is required for this server.", "8233 Inappropriate authentication.", "8234 The authentication mechanism is unknown.", "8235 A referral was returned from the server.", "8236 The server does not support the requested critical extension.", "8237 This request requires a secure connection.", "8238 Inappropriate matching.", "8239 A constraint violation occurred.", "8240 There is no such object on the server.", "8241 There is an alias problem.", "8242 An invalid dn syntax has been specified.", "8243 The object is a leaf object.", "8244 There is an alias dereferencing problem.", 
                    "8245 The server is unwilling to process the request.", "8246 A loop has been detected.", "8247 There is a naming violation.", "8248 The result set is too large.", "8249 The operation affects multiple DSAs.", "8250 The server is not operational.", "8251 A local error has occurred.", "8252 An encoding error has occurred.", "8253 A decoding error has occurred.", "8254 The search filter cannot be recognized.", "8255 One or more parameters are illegal.", "8256 The specified method is not supported.", "8257 No results were returned.", "8258 The specified control is not supported by the server.", "8259 A referral loop was detected by the client.", "8260 The preset referral limit was exceeded.", 
                    "8261 The search requires a SORT control.", "8262 The search results exceed the offset range specified.", "8301 The root object must be the head of a naming context.", "8302 The add replica operation cannot be performed.", "8303 A reference to an attribute that is not defined in the schema occurred.", "8304 The maximum size of an object has been exceeded.", "8305 An attempt was made to add an object to the directory with a name that is already in use.", "8306 An attempt was made to add an object of a class that does not have an RDN defined in the schema.", "8307 An attempt was made to add an object using an RDN that is not the RDN defined in the schema.", "8308 None of the requested attributes were found on the objects.", "8309 The user buffer is too small.", "8310 The attribute specified in the operation is not present on the object.", "8311 Illegal modify operation.", "8312 The specified object is too large.", "8313 The specified instance type is not valid.", "8314 The operation must be performed at a master DSA.", 
                    "8315 The object class attribute must be specified.", "8316 A required attribute is missing.", "8317 An attempt was made to modify an object to include an attribute that is not legal for its class.", "8318 The specified attribute is already present on the object.", "8320 The specified attribute is not present, or has no values.", "8321 Multiple values were specified for an attribute that can have only one value.", "8322 A value for the attribute was not in the acceptable range of values.", "8323 The specified value already exists.", "8324 The attribute cannot be removed because it is not present on the object.", "8325 The attribute value cannot be removed because it is not present on the object.", "8326 The specified root object cannot be a subref.", "8327 Chaining is not permitted.", "8328 Chained evaluation is not permitted.", "8329 The operation could not be performed because the object//s parent is either uninstantiated or deleted.", "8330 Having a parent that is an alias is not permitted.", "8331 The object and parent must be of the same type, either both masters or both replicas.", 
                    "8332 The operation cannot be performed because child objects exist.", "8333 Directory object not found.", "8334 The aliased object is missing.", "8335 The object name has bad syntax.", "8336 It is not permitted for an alias to refer to another alias.", "8337 The alias cannot be dereferenced.", "8338 The operation is out of scope.", "8340 The DSA object cannot be deleted.", "8341 A directory service error has occurred.", "8342 The operation can only be performed on an internal master DSA object.", "8343 The object must be of class DSA.", "8344 Insufficient access rights to perform the operation.", "8345 The object cannot be added because the parent is not on the list of possible superiors.", "8346 Access to the attribute is not permitted because the attribute is owned by the Security Accounts Manager (SAM).", "8347 The name has too many parts.", "8348 The name is too long.", 
                    "8349 The name value is too long.", "8350 The directory service encountered an error parsing a name.", "8351 The directory service cannot get the attribute type for a name.", "8352 The name does not identify an object; the name identifies a phantom.", "8353 The security descriptor is too short.", "8354 The security descriptor is invalid.", "8355 Failed to create name for deleted object.", "8356 The parent of a new subref must exist.", "8357 The object must be a naming context.", "8358 It is not permitted to add an attribute which is owned by the system.", "8359 The class of the object must be structural; you cannot instantiate an abstract class.", "8360 The schema object could not be found.", "8361 A local object with this GUID (dead or alive) already exists.", "8362 The operation cannot be performed on a back link.", "8363 The cross reference for the specified naming context could not be found.", "8364 The operation could not be performed because the directory service is shutting down.", 
                    "8365 The directory service request is invalid.", "8366 The role owner attribute could not be read.", "8367 The requested FSMO operation failed.", "8368 Modification of a DN across a naming context is not permitted.", "8369 The attribute cannot be modified because it is owned by the system.", "8370 Only the replicator can perform this function.", "8371 The specified class is not defined.", "8372 The specified class is not a subclass.", "8373 The name reference is invalid.", "8374 A cross reference already exists.", "8375 It is not permitted to delete a master cross reference.", "8376 Subtree notifications are only supported on NC heads.", "8377 Notification filter is too complex.", "8378 Schema update failed: duplicate RDN.", "8379 Schema update failed: duplicate OID.", "8380 Schema update failed: duplicate MAPI identifier.", 
                    "8381 Schema update failed: duplicate schema-id GUID.", "8382 Schema update failed: duplicate LDAP display name.", "8383 Schema update failed: range-lower less than range upper.", "8384 Schema update failed: syntax mismatch.", "8385 Schema deletion failed: attribute is used in must-contain.", "8386 Schema deletion failed: attribute is used in may-contain.", "8387 Schema update failed: attribute in may-contain does not exist.", "8388 Schema update failed: attribute in must-contain does not exist.", "8389 Schema update failed: class in aux-class list does not exist or is not an auxiliary class.", "8390 Schema update failed: class in poss-superiors does not exist.", "8391 Schema update failed: class in subclassof list does not exist or does not satisfy hierarchy rules.", "8392 Schema update failed: Rdn-Att-Id has wrong syntax.", "8393 Schema deletion failed: class is used as auxiliary class.", "8394 Schema deletion failed: class is used as sub class.", "8395 Schema deletion failed: class is used as poss superior.", "8396 Schema update failed in recalculating validation cache.", 
                    "8397 The tree deletion is not finished.", "8398 The requested delete operation could not be performed.", "8399 Cannot read the governs class identifier for the schema record.", "8400 The attribute schema has bad syntax.", "8401 The attribute could not be cached.", "8402 The class could not be cached.", "8403 The attribute could not be removed from the cache.", "8404 The class could not be removed from the cache.", "8405 The distinguished name attribute could not be read.", "8406 A required subref is missing.", "8407 The instance type attribute could not be retrieved.", "8408 An internal error has occurred.", "8409 A database error has occurred.", "8410 The attribute GOVERNSID is missing.", "8411 An expected attribute is missing.", "8412 The specified naming context is missing a cross reference.", 
                    "8413 A security checking error has occurred.", "8414 The schema is not loaded.", "8415 Schema allocation failed.", "8416 Failed to obtain the required syntax for the attribute schema.", "8417 The global catalog verification failed.", "8418 The replication operation failed because of a schema mismatch between the servers involved.", "8419 The DSA object could not be found.", "8420 The naming context could not be found.", "8421 The naming context could not be found in the cache.", "8422 The child object could not be retrieved.", "8423 The modification was not permitted for security reasons.", "8424 The operation cannot replace the hidden record.", "8425 The hierarchy file is invalid.", "8426 The attempt to build the hierarchy table failed.", "8427 The directory configuration parameter is missing from the registry.", "8428 The attempt to count the address book indices failed.", 
                    "8429 The allocation of the hierarchy table failed.", "8430 The directory service encountered an internal failure.", "8431 The directory service encountered an unknown failure.", "8432 A root object requires a class of //top//.", "8433 This directory server is shutting down, and cannot take ownership of new floating single-master operation roles.", "8434 The directory service is missing mandatory configuration information, and is unable to determine the ownership of floating single-master operation roles.", "8435 The directory service was unable to transfer ownership of one or more floating single-master operation roles to other servers.", "8436 The replication operation failed.", "8437 An invalid parameter was specified for this replication operation.", "8438 The directory service is too busy to complete the replication operation at this time.", "8439 The distinguished name specified for this replication operation is invalid.", "8440 The naming context specified for this replication operation is invalid.", "8441 The distinguished name specified for this replication operation already exists.", "8442 The replication system encountered an internal error.", "8443 The replication operation encountered a database inconsistency.", "8444 The server specified for this replication operation could not be contacted.", 
                    "8445 The replication operation encountered an object with an invalid instance type.", "8446 The replication operation failed to allocate memory.", "8447 The replication operation encountered an error with the mail system.", "8448 The replication reference information for the target server already exists.", "8449 The replication reference information for the target server does not exist.", "8450 The naming context cannot be removed because it is replicated to another server.", "8451 The replication operation encountered a database error.", "8452 The naming context is in the process of being removed or is not replicated from the specified server.", "8453 Replication access was denied.", "8454 The requested operation is not supported by this version of the directory service.", "8455 The replication remote procedure call was cancelled.", "8456 The source server is currently rejecting replication requests.", "8457 The destination server is currently rejecting replication requests.", "8458 The replication operation failed due to a collision of object names.", "8459 The replication source has been reinstalled.", "8460 The replication operation failed because a required parent object is missing.", 
                    "8461 The replication operation was preempted.", "8462 The replication synchronization attempt was abandoned because of a lack of updates.", "8463 The replication operation was terminated because the system is shutting down.", "8464 The replication synchronization attempt failed as the destination partial attribute set is not a subset of source partial attribute set.", "8465 The replication synchronization attempt failed because a master replica attempted to sync from a partial replica.", "8466 The server specified for this replication operation was contacted, but that server was unable to contact an additional server needed to complete the operation.", "8467 A schema mismatch is detected between the source and the build used during a replica install.", "8468 Schema update failed: An attribute with the same link identifier already exists.", "8469 Name translation: Generic processing error.", "8470 Name translation: Could not find the name or insufficient right to see name.", "8471 Name translation: Input name mapped to more than one output name.", "8472 Name translation: Input name found, but not the associated output format.", "8473 Name translation: Unable to resolve completely, only the domain was found.", "8474 Name translation: Unable to perform purely syntactical mapping at the client without going out to the wire.", "8475 Modification of a constructed att is not allowed.", "8476 The OM-Object-Class specified is incorrect for an attribute with the specified syntax.", 
                    "8477 The replication request has been posted; waiting for reply.", "8478 The requested operation requires a directory service, and none was available.", "8479 The LDAP display name of the class or attribute contains non-ASCII characters.", "8480 The requested search operation is only supported for base searches.", "8481 The search failed to retrieve attributes from the database.", "8482 The schema update operation tried to add a backward link attribute that has no corresponding forward link.", "8483 Source and destination of a cross domain move do not agree on the object//s epoch number.", "8484 Source and destination of a cross domain move do not agree on the object//s current name.", "8485 Source and destination of a cross domain move operation are identical.", "8486 Source and destination for a cross domain move are not in agreement on the naming contexts in the forest.", "8487 Destination of a cross domain move is not authoritative for the destination naming context.", "8488 Source and destination of a cross domain move do not agree on the identity of the source object.", "8489 Object being moved across domains is already known to be deleted by the destination server.", "8490 Another operation which requires exclusive access to the PDC PSMO is already in progress.", "8491 A cross domain move operation failed such that the two versions of the moved object exist - one each in the source and destination domains.", "8492 This object may not be moved across domain boundaries either because cross domain moves for this class are disallowed, or the object has some special characteristics, eg: trust account or restricted RID, which prevent its move.", 
                    "8493 Can//t move objects with memberships across domain boundaries as once moved, this would violate the membership conditions of the account group.", "8494 A naming context head must be the immediate child of another naming context head, not of an interior node.", "8495 The directory cannot validate the proposed naming context name because it does not hold a replica of the naming context above the proposed naming context.", "8496 Destination domain must be in native mode.", "8497 The operation can not be performed because the server does not have an infrastructure container in the domain of interest.", "8498 Cross domain move of account groups is not allowed.", "8499 Cross domain move of resource groups is not allowed.", "8500 The search flags for the attribute are invalid.", "8501 Tree deletions starting at an object which has an NC head as a descendant are not allowed.", "8502 The directory service failed to lock a tree in preparation for a tree deletion because the tree was in use.", "8503 The directory service failed to identify the list of objects to delete while attempting a tree deletion.", "8504 Security Accounts Manager initialization failed because of an error.", "8505 Only an administrator can modify the membership list of an administrative group.", "8506 Cannot change the primary group ID of a domain controller account.", "8507 An attempt is made to modify the base schema.", "8508 Adding a new mandatory attribute to an existing class, deleting a mandatory attribute from an existing class, or adding an optional attribute to the special class Top that is not a backlink attribute (directly or through inheritance, for example, by adding or deleting an auxiliary class) is not allowed.", 
                    "8509 Schema update is not allowed on this DC.", "8510 An object of this class cannot be created under the schema container.", "8511 The replica/child install failed to get the objectVersion attribute on the schema container on the source DC.", "8512 The replica/child install failed to read the objectVersion attribute in the SCHEMA section of the file schema.", "8513 The specified group type is invalid.", "8514 Cannot nest global groups in a mixed domain if the group is security-enabled.", "8515 Cannot nest local groups in a mixed domain if the group is security-enabled.", "8516 A global group cannot have a local group as a member.", "8517 A global group cannot have a universal group as a member.", "8518 A universal group cannot have a local group as a member.", "8519 A global group cannot have a cross-domain member.", "8520 A local group cannot have another cross-domain local group as a member.", "8521 A group with primary members cannot change to a security-disabled group.", "8522 The schema cache load failed to convert the string default SD on a class-schema object.", "8523 Only DSAs configured to be Global Catalog servers should be allowed to hold the Domain Naming Master FSMO role.", "8524 The DSA operation is unable to proceed because of a DNS lookup failure.", 
                    "8525 While processing a change to the DNS Host Name for an object, the Service Principal Name values could not be kept in sync.", "8526 The Security Descriptor attribute could not be read.", "8527 The object requested was not found, but an object with that key was found.", "8528 The syntax of the linked attributed being added is incorrect.", "8529 Security Account Manager needs to get the boot password.", "8530 Security Account Manager needs to get the boot key from floppy disk.", "8531 Directory Service cannot start.", "8532 Directory Services could not start.", "8533 The connection between client and server requires packet privacy or better.", "8534 The source domain may not be in the same forest as destination.", "8535 The destination domain must be in the forest.", "8536 The operation requires that destination domain auditing be enabled.", "8537 The operation couldn//t locate a DC for the source domain.", "8538 The source object must be a group or user.", "8539 The source object//s SID already exists in destination forest.", "8540 The source and destination object must be of the same type.", 
                    "8541 Security Accounts Manager initialization failed because of an error.", "8542 Schema information could not be included in the replication request.", "8543 The replication operation could not be completed due to a schema incompatibility.", "8544 The replication operation could not be completed due to a previous schema incompatibility.", "8545 The replication update could not be applied because either the source or the destination has not yet received information regarding a recent cross-domain move operation.", "8546 The requested domain could not be deleted because there exist domain controllers that still host this domain.", "8547 The requested operation can be performed only on a global catalog server.", "8548 A local group can only be a member of other local groups in the same domain.", "8549 Foreign security principals cannot be members of universal groups.", "8550 The attribute is not allowed to be replicated to the GC because of security reasons.", "8551 The checkpoint with the PDC could not be taken because there are too many modifications being processed currently.", "8552 The operation requires that source domain auditing be enabled.", "8553 Security principal objects can only be created inside domain naming contexts.", "8554 A Service Principal Name (SPN) could not be constructed because the provided hostname is not in the necessary format.", "8555 A Filter was passed that uses constructed attributes.", "8556 The unicodePwd attribute value must be enclosed in double quotes.", 
                    "8557 Your computer could not be joined to the domain.", "8558 For security reasons, the operation must be run on the destination DC.", "8559 For security reasons, the source DC must be NT4SP4 or greater.", "8560 Critical Directory Service System objects cannot be deleted during tree delete operations.", "8561 Directory Services could not start because of an error.", "8562 Security Accounts Manager initialization failed because of an error.", "9001 DNS server unable to interpret format.", "9002 DNS server failure.", "9003 DNS name does not exist.", "9004 DNS request not supported by name server.", "9005 DNS operation refused.", "9006 DNS name that ought not exist, does exist.", "9007 DNS RR set that ought not exist, does exist.", "9008 DNS RR set that ought to exist, does not exist.", "9009 DNS server not authoritative for zone.", "9010 DNS name in update or prereq is not in zone.", 
                    "9016 DNS signature failed to verify.", "9017 DNS bad key.", "9018 DNS signature validity expired.", "9501 No records found for given DNS query.", "9502 Bad DNS packet.", "9503 No DNS packet.", "9504 DNS error, check rcode.", "9505 Unsecured DNS packet.", "9551 Invalid DNS type.", "9552 Invalid IP address.", "9553 Invalid property.", "9554 Try DNS operation again later.", "9555 Record for given name and type is not unique.", "9556 DNS name does not comply with RFC specifications.", "9557 DNS name is a fully-qualified DNS name.", "9558 DNS name is dotted (multi-label).", 
                    "9559 DNS name is a single-part name.", "9560 DSN name contains an invalid character.", "9561 DNS name is entirely numeric.", "9601 DNS zone does not exist.", "9602 DNS zone information not available.", "9603 Invalid operation for DNS zone.", "9604 Invalid DNS zone configuration.", "9605 DNS zone has no start of authority (SOA) record.", "9606 DNS zone has no name server (NS) record.", "9607 DNS zone is locked.", "9608 DNS zone creation failed.", "9609 DNS zone already exists.", "9610 DNS automatic zone already exists.", "9611 Invalid DNS zone type.", "9612 Secondary DNS zone requires master IP address.", "9613 DNS zone not secondary.", 
                    "9614 Need secondary IP address.", "9615 WINS initialization failed.", "9616 Need WINS servers.", "9617 NBTSTAT initialization call failed.", "9618 Invalid delete of start of authority (SOA).", "9651 Primary DNS zone requires datafile.", "9652 Invalid datafile name for DNS zone.", "9653 Failed to open datafile for DNS zone.", "9654 Failed to write datafile for DNS zone.", "9655 Failure while reading datafile for DNS zone.", "9701 DNS record does not exist.", "9702 DNS record format error.", "9703 Node creation failure in DNS.", "9704 Unknown DNS record type.", "9705 DNS record timed out.", "9706 Name not in DNS zone.", 
                    "9707 CNAME loop detected.", "9708 Node is a CNAME DNS record.", "9709 A CNAME record already exists for given name.", "9710 Record only at DNS zone root.", "9711 DNS record already exists.", "9712 Secondary DNS zone data error.", "9713 Could not create DNS cache data.", "9714 DNS name does not exist.", "9715 Could not create pointer (PTR) record.", "9716 DNS domain was undeleted.", "9717 The directory service is unavailable.", "9718 DNS zone already exists in the directory service.", "9719 DNS server not creating or reading the boot file for the directory service integrated DNS zone.", "9751 DNS AXFR (zone transfer) complete.", "9752 DNS zone transfer failed.", "9753 Added local WINS server.", 
                    "9801 Secure update call needs to continue update request.", "9851 TCP/IP network protocol not installed.", "9852 No DNS servers configured for local system.", "10004 A blocking operation was interrupted by a call to WSACancelBlockingCall.", "10009 The file handle supplied is not valid.", "10013 An attempt was made to access a socket in a way forbidden by its access permissions.", "10014 The system detected an invalid pointer address in attempting to use a pointer argument in a call.", "10022 An invalid argument was supplied.", "10024 Too many open sockets.", "10035 A non-blocking socket operation could not be completed immediately.", "10036 A blocking operation is currently executing.", "10037 An operation was attempted on a non-blocking socket that already had an operation in progress.", "10038 An operation was attempted on something that is not a socket.", "10039 A required address was omitted from an operation on a socket.", "10040 A message sent on a datagram socket was larger than the internal message buffer or some other network limit, or the buffer used to receive a datagram into was smaller than the datagram itself.", "10041 A protocol was specified in the socket function call that does not support the semantics of the socket type requested.", 
                    "10042 An unknown, invalid, or unsupported option or level was specified in a getsockopt or setsockopt call.", "10043 The requested protocol has not been configured into the system, or no implementation for it exists.", "10044 The support for the specified socket type does not exist in this address family.", "10045 The attempted operation is not supported for the type of object referenced.", "10046 The protocol family has not been configured into the system or no implementation for it exists.", "10047 An address incompatible with the requested protocol was used.", "10048 Only one usage of each socket address (protocol/network address/port) is normally permitted.", "10049 The requested address is not valid in its context.", "10050 A socket operation encountered a dead network.", "10051 A socket operation was attempted to an unreachable network.", "10052 The connection has been broken due to keep-alive activity detecting a failure while the operation was in progress.", "10053 An established connection was aborted by the software in your host machine.", "10054 An existing connection was forcibly closed by the remote host.", "10055 An operation on a socket could not be performed because the system lacked sufficient buffer space or because a queue was full.", "10056 A connect request was made on an already connected socket.", "10057 A request to send or receive data was disallowed because the socket is not connected and (when sending on a datagram socket using a sendto call) no address was supplied.", 
                    "10058 A request to send or receive data was disallowed because the socket had already been shut down in that direction with a previous shutdown call.", "10059 Too many references to some kernel object.", "10060 A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond.", "10061 No connection could be made because the target machine actively refused it.", "10062 Cannot translate name.", "10063 Name component or name was too long.", "10064 A socket operation failed because the destination host was down.", "10065 A socket operation was attempted to an unreachable host.", "10066 Cannot remove a directory that is not empty.", "10067 A Windows Sockets implementation may have a limit on the number of applications that may use it simultaneously.", "10068 Ran out of quota.", "10069 Ran out of disk quota.", "10070 File handle reference is no longer available.", "10071 Item is not available locally.", "10091 WSAStartup cannot function at this time because the underlying system it uses to provide network services is currently unavailable.", "10092 The Windows Sockets version requested is not supported.", 
                    "10093 Either the application has not called WSAStartup, or WSAStartup failed.", "10101 Returned by WSARecv or WSARecvFrom to indicate the remote party has initiated a graceful shutdown sequence.", "10102 No more results can be returned by WSALookupServiceNext.", "10103 A call to WSALookupServiceEnd was made while this call was still processing.", "10104 The procedure call table is invalid.", "10105 The requested service provider is invalid.", "10106 The requested service provider could not be loaded or initialized.", "10107 A system call that should never fail has failed.", "10108 No such service is known.", "10109 The specified class was not found.", "10110 No more results can be returned by WSALookupServiceNext.", "10111 A call to WSALookupServiceEnd was made while this call was still processing.", "10112 A database query failed because it was actively refused.", "11001 No such host is known.", "11002 This is usually a temporary error during hostname resolution and means that the local server did not receive a response from an authoritative server.", "11003 A non-recoverable error occurred during a database lookup.", 
                    "11004 The requested name is valid and was found in the database, but it does not have the correct associated data being resolved for.", "11005 At least one reserve has arrived.", "11006 At least one path has arrived.", "11007 There are no senders.", "11008 There are no receivers.", "11009 Reserve has been confirmed.", "11010 Error due to lack of resources.", "11011 Rejected for administrative reasons - bad credentials.", "11012 Unknown or conflicting style.", "11013 Problem with some part of the filterspec or providerspecific buffer in general.", "11014 Problem with some part of the flowspec.", "11015 General QOS error.", "11016 An invalid or unrecognized service type was found in the flowspec.", "11017 An invalid or inconsistent flowspec was found in the QOS structure.", "11018 Invalid QOS provider-specific buffer.", "11019 An invalid QOS filter style was used.", 
                    "11020 An invalid QOS filter type was used.", "11021 An incorrect number of QOS FILTERSPECs were specified in the FLOWDESCRIPTOR.", "11022 An object with an invalid ObjectLength field was specified in the QOS provider-specific buffer.", "11023 An incorrect number of flow descriptors was specified in the QOS structure.", "11024 An unrecognized object was found in the QOS provider-specific buffer.", "11025 An invalid policy object was found in the QOS provider-specific buffer.", "11026 An invalid QOS flow descriptor was found in the flow descriptor list.", "11027 An invalid or inconsistent flowspec was found in the QOS provider-specific buffer.", "11028 An invalid FILTERSPEC was found in the QOS provider-specific buffer.", "11029 An invalid shape discard mode object was found in the QOS provider-specific buffer.", "11030 An invalid shaping rate object was found in the QOS provider-specific buffer.", "11031 A reserved policy element was found in the QOS provider-specific buffer."
                 };

                internal static string GetText(int num)
                {
                    foreach (string str in Codes)
                    {
                        int index = str.IndexOf(" ");
                        if (Convert.ToInt32(str.Substring(0, index)) == num)
                        {
                            return str.Substring(index + 1);
                        }
                    }
                    return "";
                }
            }
        }
    }
}

