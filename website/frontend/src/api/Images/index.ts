import axios from "axios";
import { ImageFile, User } from "@/typings";

export const uploadImages = async (images: ImageFile[]): Promise<User> => {
    const { data }: { data: any } = await axios.post("/api/imageupload", {
        ImageList: images,
    });
    return { ...data };
};
